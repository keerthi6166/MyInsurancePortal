using MyInsurancePortal.Interfaces;
using MyInsurancePortal.Models;
using MyInsurancePortal.DtoModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace MyInsurancePortal.Controllers
{
    [ApiController] // Marks this as a Web API controller
    [Route("api/[controller]")]
    public class ClaimController : ControllerBase //controller base is used to return methods like notfound(), ok(), badrequest(object), createdataction(), ModelState
    {
        private readonly InsuranceDbContext _context;
        private readonly IClaimService _claimService;

        public ClaimController(IClaimService claimService, InsuranceDbContext context)
        {
            _claimService = claimService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClaims()
        {
            var result = await _claimService.GetAllClaims();
            return Ok(result); // no need to handle exceptions as it will be handled by globalmiddlewareExceptions

        }

        [HttpGet("{claimNumber}")] //GET /api/claim/1
        public async Task<IActionResult> GetClaimById(string claimNumber)
        {
            var result = await _claimService.GetClaimByClaimNumber(claimNumber);
            return Ok(result);
        }

        [HttpGet("policy/{policyNumber}")]
        public async Task<IActionResult> GetClaimsByPolicyId(string policyNumber)
        {
            var result = await _claimService.GetClaimsByPolicyNumber(policyNumber);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewClaim([FromBody] ClaimDto claim)
        {
            var result = await _claimService.AddNewClaim(claim);
            return Ok(result);
        }

        [HttpPut("{claimNumber}")]
        public async Task<IActionResult> UpdateClaim(string claimNumber, [FromBody] ClaimDto claim)
        {
            var result = await _claimService.UpdateClaim(claimNumber, claim);
            return Ok(result);
        }

        [HttpDelete("{claimNumber}")]
        public async Task<IActionResult> DeleteClaim(string claimNumber)
        {
            var result = await _claimService.DeleteClaim(claimNumber);
            return Ok(result);
        }
    }
}
