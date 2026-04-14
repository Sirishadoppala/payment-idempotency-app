using System.ComponentModel.DataAnnotations;

namespace Payment_Idempotency_Service_Backend.DTOs
{
    public class PaymentRequestDTO
    {
        [Required]
        public string? PaymentId {  get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount {  get; set; }

    }
}
