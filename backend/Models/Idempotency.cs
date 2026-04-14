namespace Payment_Idempotency_Service_Backend.Models
{
    public class Idempotency
    {
        public string? IdempotencyId {  get; set; }
        public string? IdempotencyKey {  get; set; }
        public string? RequestHash {  get; set; }
        public string? ResponseBody {  get; set; }
        public string? Status {  get; set; }
        public DateTimeOffset CreatedAt {  get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
    }
}
