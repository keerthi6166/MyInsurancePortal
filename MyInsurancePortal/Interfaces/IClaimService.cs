using MyInsurancePortal.DtoModels;

namespace MyInsurancePortal.Interfaces
{
    public interface IClaimService
    {
        Task<List<ClaimDto>> GetAllClaims();
        Task<ClaimDto?> GetClaimByClaimNumber(string claimNumber);
        Task<List<ClaimDto>> GetClaimsByPolicyNumber(string policyNumber);
        Task<ClaimDto> AddNewClaim(ClaimDto claimDto);
        Task<ClaimDto> UpdateClaim(string claimNumber, ClaimDto claimDto);
        Task<bool> DeleteClaim(string claimNumber);
    }
}
