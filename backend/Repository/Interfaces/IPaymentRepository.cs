using Payment_Idempotency_Service_Backend.Models;

namespace Payment_Idempotency_Service_Backend.Repository.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> GetByPaymentIdAsync(string paymentId);
        Task<Payment?> CreateAsync(Payment payment);
    }
}
