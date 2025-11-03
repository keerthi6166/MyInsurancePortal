using MyInsurancePortal.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MyInsurancePortal.Models;
using MyInsurancePortal.DtoModels;

namespace MyInsurancePortal.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var result = await _customerService.GetAllCustomers();
            return Ok(result);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetCustomerById(string email)
        {
            var result = await _customerService.GetCustomerByEmail(email);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewCustomer([FromBody] CustomerDto cutomer)// no need to add model validation separately becaue it will be added by data annotations.
        {
            var result = await _customerService.AddNewCustomer(cutomer);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerDto customer)
        {            
            var result = await _customerService.UpdateCustomer(customer);
            return Ok(result);
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteCustomer(string email)
        {
            var result = await _customerService.DeleteCustomer(email);
            return Ok(result);
        }
    }
}
