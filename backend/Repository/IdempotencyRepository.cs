using Microsoft.EntityFrameworkCore;
using Payment_Idempotency_Service_Backend.Data;
using Payment_Idempotency_Service_Backend.Models;
using Payment_Idempotency_Service_Backend.Repository.Interfaces;
namespace Payment_Idempotency_Service_Backend.Repository
{
    public class IdempotencyRepository : IIdempotencyRepository
    {
        private readonly AppDbContext _appDbContext;
        public IdempotencyRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Idempotency?> GetByKeyAsync(string key)
        {
            return await _appDbContext.Idempotency.AsNoTracking().FirstOrDefaultAsync(x=>x.IdempotencyKey==key);
        }
        public async Task SetResultAsync(Idempotency idempotency) 
        {
            await _appDbContext.Idempotency.AddAsync(idempotency);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
