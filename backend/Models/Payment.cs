namespace Payment_Idempotency_Service_Backend.Models
{
    public class Payment
    {
        public string? PaymentId {  get; set; }
        public string? UserId {  get; set; }
        public decimal Amount {  get; set; }
        public string? Status {  get; set; }
        public DateTimeOffset CreatedAt {  get; set; }
    }
}
