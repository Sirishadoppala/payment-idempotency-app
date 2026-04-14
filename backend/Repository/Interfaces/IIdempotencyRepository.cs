using Payment_Idempotency_Service_Backend.Models;

namespace Payment_Idempotency_Service_Backend.Repository.Interfaces
{
    public interface IIdempotencyRepository
    {
       Task<Idempotency?> GetByKeyAsync(string key);
       Task SetResultAsync(Idempotency idempotency);
    }
}