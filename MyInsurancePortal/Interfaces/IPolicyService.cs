using MyInsurancePortal.Models;
using MyInsurancePortal.DtoModels;

namespace MyInsurancePortal.Interfaces
{
    public interface IPolicyService
    {
        Task<List<PolicyDto>> GetAllPolicies();
        Task<PolicyDto?> GetPolicyByPolicyNumber(string policyNumber);
        Task<List<PolicyDto>> GetPolicyByCustomerEmail(string email); // custom
        Task<PolicyDto> AddNewPolicy(PolicyDto policy);
        Task<PolicyDto> UpdatePolicy(PolicyDto policy);
        Task<bool> DeletePolicy(string policyNumber);
    }
}
