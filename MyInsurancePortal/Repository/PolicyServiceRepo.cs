using AutoMapper;
using MyInsurancePortal.Models;
using MyInsurancePortal.DtoModels;
using MyInsurancePortal.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyInsurancePortal.Exceptions;

namespace MyInsurancePortal.Repository
{
    public class PolicyServiceRepo : IPolicyService
    {
        private readonly InsuranceDbContext _context;
        private readonly ILogger<PolicyServiceRepo> _logger;
        private readonly IMapper _mapper;

        public PolicyServiceRepo(InsuranceDbContext context, ILogger<PolicyServiceRepo> logger,IMapper mapper) 
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<PolicyDto>> GetAllPolicies()
        {
            var policies = await _context.Policies.ToListAsync();
            if (policies == null || policies.Count == 0)
            {
                throw new KeyNotFoundException(PolicyExceptions.NoPoliciesFound);
            }
            _context.SaveChanges();
            return _mapper.Map<List<PolicyDto>>(policies);    
        }

        public async Task<PolicyDto?> GetPolicyByPolicyNumber(string policyNumber)
        {
            var policy = await _context.Policies.Where(x=>x.PolicyNumber == policyNumber).FirstOrDefaultAsync();
            if (policy == null)
            {
                throw new KeyNotFoundException(PolicyExceptions.PolicyNotFound);
            }
                return _mapper?.Map<PolicyDto>(policy);
        }
        public async Task<List<PolicyDto>> GetPolicyByCustomerEmail(string email)
        {
            var policies = await _context.Policies
                .Include(x=>x.Customer)
                .Where(x => x.Customer.Email == email).ToListAsync();

            if (policies == null || policies.Count == 0)
            {
                throw new KeyNotFoundException(PolicyExceptions.NoPoliciesFound);
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<List<PolicyDto>>(policies);
        }
        public async Task<PolicyDto> AddNewPolicy(PolicyDto policyDto)
        {
            var policy = _mapper.Map<Policy>(policyDto);
            _context.Policies.Add(policy);
            await _context.SaveChangesAsync();
            return _mapper.Map<PolicyDto>(policy);
        }
        public async Task<PolicyDto> UpdatePolicy(PolicyDto policyDto)
        {
            var exisitngPolicy = await _context.Policies
                .FirstOrDefaultAsync(x=>x.PolicyNumber == policyDto.PolicyNumber);

            if (exisitngPolicy == null)
            { 
                throw new KeyNotFoundException(PolicyExceptions.PolicyNotFound);
            }
            _mapper.Map(policyDto, exisitngPolicy);
            await _context.SaveChangesAsync();
            return _mapper.Map<PolicyDto>(exisitngPolicy);
        }
        public async Task<bool> DeletePolicy(string policyNumber)
        {
            var policy = await _context.Policies.FirstOrDefaultAsync(x=>x.PolicyNumber == policyNumber);

            if (policy == null)
                throw new KeyNotFoundException(PolicyExceptions.PolicyNotFound);

            _context.Remove(policy);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
