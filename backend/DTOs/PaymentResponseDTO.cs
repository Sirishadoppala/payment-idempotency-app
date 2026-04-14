namespace Payment_Idempotency_Service_Backend.DTOs
{
    public class PaymentResponseDTO
    {
        public string? PaymentId {  get; set; }
        public String? Status {  get; set; }
        public String? Message { get; set; }
    }
}
