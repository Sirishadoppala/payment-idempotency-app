using Azure.Core;
using Payment_Idempotency_Service_Backend.DTOs;
using Payment_Idempotency_Service_Backend.Models;
using Payment_Idempotency_Service_Backend.Repository;
using Payment_Idempotency_Service_Backend.Repository.Interfaces;
using System.Text;

namespace Payment_Idempotency_Service_Backend.Service
{
    public class PaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IIdempotencyRepository _idempotencyrepository;
       public PaymentService(IPaymentRepository paymentRepository,IIdempotencyRepository idempotencyRepository)
        {
            _paymentRepository = paymentRepository;
            _idempotencyrepository = idempotencyRepository;
        }
        public async Task<PaymentResponseDTO> ProcessPaymentAsync(PaymentRequestDTO request, string key)
        {
            // canonicalize request and compute SHA256 hash
            var requestData = $"paymentId:{request.PaymentId};amount:{request.Amount}";
            using var sha = System.Security.Cryptography.SHA256.Create();
            var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(requestData));
            var requestHash = Convert.ToBase64String(hashBytes);

            var existingKey = await _idempotencyrepository.GetByKeyAsync(key);

            if (existingKey != null)
            {
                if (existingKey.RequestHash != requestHash)
                {
                    throw new Exception("Request mismatch for same idempotency key");
                }
                return new PaymentResponseDTO
                {
                    PaymentId = request.PaymentId,
                    Status = existingKey.Status,
                    Message = existingKey.ResponseBody
                };
            }

            var existingPayment = await _paymentRepository.GetByPaymentIdAsync(request.PaymentId);

            if (existingPayment != null)
            {
                throw new InvalidOperationException("Payment already exists");
            }

            var payment = new Payment
            {
                PaymentId = request.PaymentId,
                Amount = request.Amount,
                Status = "Succeeded",
                CreatedAt = DateTimeOffset.UtcNow
            };
            var idempotency = new Idempotency
            {
                IdempotencyId = Guid.NewGuid().ToString(),
                IdempotencyKey = key,
                RequestHash = requestHash,
                ResponseBody = "Payment Successful",
                Status = "Succeeded",
                CreatedAt = DateTimeOffset.UtcNow,
                ExpiresAt = DateTimeOffset.UtcNow.AddHours(1)
            };

            // Try to create payment and idempotency; handle unique constraint
            try
            {
                await _paymentRepository.CreateAsync(payment);
                await _idempotencyrepository.SetResultAsync(idempotency);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                // likely a unique constraint violation — treat as conflict
                throw new InvalidOperationException("Conflict creating payment or idempotency");
            }

            // 🔹 Step 6: Return response
            return new PaymentResponseDTO
            {
                PaymentId = payment.PaymentId,
                Status = payment.Status,
                Message = "Payment Successful"
            };
        }
        public async Task<Payment?> GetByPaymentIdAsync(string paymentId)
        {
            var result = await _paymentRepository.GetByPaymentIdAsync(paymentId);
            return result;
        }
    }
}
