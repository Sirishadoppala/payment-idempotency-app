using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Payment_Idempotency_Service_Backend.DTOs;
using Payment_Idempotency_Service_Backend.Repository;
using Payment_Idempotency_Service_Backend.Repository.Interfaces;
using Payment_Idempotency_Service_Backend.Service;

namespace Payment_Idempotency_Service_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        private readonly IdempotencyService _idempotencyService;
        public PaymentController(PaymentService paymentService,IdempotencyService idempotencyService)
        {
            _paymentService = paymentService;
            _idempotencyService= idempotencyService;
        }

        [HttpPost("Process")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDTO request, [FromHeader(Name = "Idempotency-Key")] string key)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                return BadRequest("Idempotency-Key header is required");
            }
            try
            {
                await _idempotencyService.CreateKeyAsync(key);

                var result = await _paymentService.ProcessPaymentAsync(request,key);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred");
            }
        }
        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPaymentStatus(string paymentId)
        {
            var payment = await _paymentService.GetByPaymentIdAsync(paymentId);
            if (payment == null)
                return NotFound();

            return Ok(payment);
        }
    }
}
