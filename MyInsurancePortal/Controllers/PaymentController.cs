using MyInsurancePortal.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MyInsurancePortal.Models;
using MyInsurancePortal.DtoModels;

namespace MyInsurancePortal.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService) 
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetallPayments()
        {
            var result = await _paymentService.GetAllPayments();
            return Ok(result);
        }

        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetPaymentByTransactionId(string transactionId)
        {
            var result = await _paymentService.GetPaymentByTransactionId(transactionId);
            return Ok(result);
        }

        [HttpGet("policy/{policyNumber}")]
        public async Task<IActionResult> GetPaymentsByPolicyNumber(string policyNumber)
        {
            var result = await _paymentService.GetPaymentsByPolicyNumber(policyNumber);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddNewPayment([FromBody] PaymentDto payment)
        {
            var result = await _paymentService.AddNewPayment(payment);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePayment([FromBody] PaymentDto payment)
        {            
            var result = await _paymentService.UpdatePayment(payment);
            return Ok(result);
        }

        [HttpDelete("{transactionId}")]
        public async Task<IActionResult> DeletePayment(string transactionId)
        {
            var result = await _paymentService.DeletePayment(transactionId); return Ok(result);
        }
    }
}
