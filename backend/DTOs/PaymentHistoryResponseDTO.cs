using System.Runtime.InteropServices;

namespace Payment_Idempotency_Service_Backend.DTOs
{
    public class PaymentHistoryResponseDTO
    {
        public string? paymentId {  get; set; }

        public String? userId {  get; set; }

        public Double amount { get; set; }

        public String? status { get; set; }

        public DateTimeOffset createdAt {  get; set; }

    }
}
