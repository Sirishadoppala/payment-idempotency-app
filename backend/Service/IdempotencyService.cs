using Payment_Idempotency_Service_Backend.Models;
using Payment_Idempotency_Service_Backend.Repository;
using Payment_Idempotency_Service_Backend.Repository.Interfaces;

namespace Payment_Idempotency_Service_Backend.Service
{
    public class IdempotencyService
    {
        private readonly IIdempotencyRepository _idempotencyRepository;
        public IdempotencyService(IIdempotencyRepository idempotencyRepository)
        {
            _idempotencyRepository = idempotencyRepository;
        }
        public async Task<Idempotency> GetDetailsByIdAsync(string key)
        {
            if(key==null) throw new ArgumentNullException(nameof(key));
            var result=await _idempotencyRepository.GetByKeyAsync(key);
            return result;
        }

        public async Task CreateKeyAsync(string key)
        {
            var existing = await _idempotencyRepository.GetByKeyAsync(key);
            if(existing!=null)
            {
                throw new InvalidOperationException("Duplicate request");
            }
            existing = new Idempotency()
            {
                IdempotencyId = Guid.NewGuid().ToString(),
                IdempotencyKey = key,
                CreatedAt = DateTimeOffset.UtcNow,
            };
            await _idempotencyRepository.SetResultAsync(existing);

        }
    }
}
