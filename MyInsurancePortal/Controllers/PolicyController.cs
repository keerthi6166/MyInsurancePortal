using MyInsurancePortal.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MyInsurancePortal.Models;
using MyInsurancePortal.DtoModels;

namespace MyInsurancePortal.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _policyService;
        public PolicyController(IPolicyService policyService)
        {
            _policyService = policyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPolicies()
        {
            var policies = await _policyService.GetAllPolicies();
            return Ok(policies);
        }

        [HttpGet("{policyNumber}")]
        public async Task<IActionResult> GetPolicyByPolicyNumber(string policyNumber)
        {
            var policy = await _policyService.GetPolicyByPolicyNumber(policyNumber);
            return Ok(policy);
        }

        [HttpGet("customer/{email}")]
        public async Task<IActionResult> GetPoliciesByCustomerEmail(string email)
        {
            var result = await _policyService.GetPolicyByCustomerEmail(email);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewPolicy([FromBody] PolicyDto policy)
        {
            var result = await _policyService.AddNewPolicy(policy);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdatePolicy([FromBody] PolicyDto policy)
        {            
            var result = await _policyService.UpdatePolicy(policy);
            return Ok(result);
        }

        [HttpDelete("{policyNumber}")]
        public async Task<IActionResult> DeletePolicy(string policyNumber)
        {
            var result = await _policyService.DeletePolicy(policyNumber);
            return Ok(result);
        }

    }
}
