using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MyInsurancePortal.Repository;
using MyInsurancePortal.Models;
using MyInsurancePortal.DtoModels;
using MyInsurancePortalTest.TestHelper;

namespace MyInsurancePortal.Tests
{
    public class PolicyServiceRepoTests
    {
        private readonly IMapper _mapper;       
        private readonly InsuranceDbContext _context;
        private readonly PolicyServiceRepo _policyRepo;

        public PolicyServiceRepoTests()
        {
            _mapper = MapperHelper.GetMapper();          
            var logger = new Mock<ILogger<PolicyServiceRepo>>().Object;

            _context = DbContextHelper.GetInMemoryDbContext();

            // 🧩 Initialize repository
            _policyRepo = new PolicyServiceRepo(_context, logger, _mapper);

             SeedData();
        }

        private void SeedData()
        {
            _context.Policies.RemoveRange(_context.Policies); // clear existing data
            _context.SaveChanges();

            var policies = new List<Policy>
            {
                new Policy
                {
                    PolicyNumber = "POL001",
                    PolicyType = "Health",
                    PremiumAmount = 1000,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddYears(1),
                    Status = "Active",
                    CustomerId = 1
                },
                new Policy
                {
                    PolicyNumber = "POL002",
                    PolicyType = "Vehicle",
                    PremiumAmount = 2000,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddYears(1),
                    Status = "Active",
                    CustomerId = 2
                }
            };
            _context.Policies.AddRange(policies);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllPolicies_ShouldReturnPolicies_WhenDataExists()
        {           
            var result = await _policyRepo.GetAllPolicies();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllPolicies_ShouldThrow_WhenNoPoliciesFound()
        {
            _context.Policies.RemoveRange(_context.Policies);
            await _context.SaveChangesAsync();

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _policyRepo.GetAllPolicies());
            Assert.Equal("No policies found.", exception.Message);
        }

        [Fact]
        public async Task GetPolicyByPolicyNumber_ShouldReturnPolicy_WhenFound()
        {

            var result = await _policyRepo.GetPolicyByPolicyNumber("POL001");

            Assert.NotNull(result);
            Assert.Equal("Health", result.PolicyType);
        }

        [Fact]
        public async Task GetPolicyByPolicyNumber_ShouldThrow_WhenNotFound()
        {

           var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _policyRepo.GetPolicyByPolicyNumber("INVALID"));
            Assert.Equal("Policy not found.", exception.Message); 
        }

        [Fact]
        public async Task AddNewPolicy_ShouldAddSuccessfully()
        {
            var policyDto = new PolicyDto
            {
                PolicyNumber = "POL003",
                PolicyType = "Life",
                PremiumAmount = 1500,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1),
                Status = "Active",
                CustomerId = 3
            };

            var result = await _policyRepo.AddNewPolicy(policyDto);

            Assert.NotNull(result);
            Assert.Equal("Life", result.PolicyType);
        }

        [Fact]
        public async Task UpdatePolicy_ShouldUpdateSuccessfully()
        {

            var updatedPolicy = new PolicyDto
            {
                PolicyNumber = "POL001",
                PolicyType = "Health Premium",
                PremiumAmount = 5000,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(2),
                Status = "Active",
                CustomerId = 1
            };

            var result = await _policyRepo.UpdatePolicy(updatedPolicy);

            Assert.NotNull(result);
            Assert.Equal("Health Premium", result.PolicyType);
            Assert.Equal(5000, result.PremiumAmount);
        }

        [Fact]
        public async Task DeletePolicy_ShouldDeleteSuccessfully()
        {

            var result = await _policyRepo.DeletePolicy("POL001");

            Assert.True(result);
            Assert.False(_context.Policies.Any(p => p.PolicyNumber == "POL001"));
        }

        [Fact]
        public async Task DeletePolicy_ShouldThrow_WhenPolicyNotFound()
        {

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _policyRepo.DeletePolicy("INVALID"));
           Assert.Equal("Policy not found.", exception.Message);
        }
    }
}
