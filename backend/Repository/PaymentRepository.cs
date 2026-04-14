using Microsoft.EntityFrameworkCore;
using Payment_Idempotency_Service_Backend.Data;
using Payment_Idempotency_Service_Backend.Models;
using Payment_Idempotency_Service_Backend.Repository.Interfaces;

namespace Payment_Idempotency_Service_Backend.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _appdbcontext;
        public PaymentRepository(AppDbContext appdbcontext)
        {
            _appdbcontext = appdbcontext;
        }

        public async Task<Payment?> GetByPaymentIdAsync(string paymentId)
        {
            return await _appdbcontext.Payments.AsNoTracking().FirstOrDefaultAsync(x => x.PaymentId == paymentId);
        }
        public async Task<Payment> CreateAsync(Payment payment) 
        {
            await _appdbcontext.Payments.AddAsync(payment);
            await _appdbcontext.SaveChangesAsync();
            return payment;
        }
    }
}
