using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(string id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDto paymentDto)
        {
            await _paymentService.AddPaymentAsync(paymentDto);
            return CreatedAtAction(nameof(GetPaymentById), new { id = paymentDto.PaymentId }, paymentDto);
        }
    }
}
