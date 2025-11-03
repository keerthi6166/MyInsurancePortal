using AutoMapper;
using MyInsurancePortal.DtoModels;
using MyInsurancePortal.Models;
using MyInsurancePortal.Repository;
using MyInsurancePortalTest.TestHelper;
using Microsoft.Extensions.Logging;
using Moq;

namespace MyInsurancePortalTest.services
{
    public class ClaimServiceRepoTests
    {
        private readonly InsuranceDbContext _context;
        private readonly IMapper _mapper;
        private readonly ClaimServiceRepo _claimRepo;

        public ClaimServiceRepoTests()
        {
            //new inmemory database instance 
            _context = DbContextHelper.GetInMemoryDbContext();
            //new mapper instance
            _mapper = MapperHelper.GetMapper();

            //logger instance
            var logger = new Mock<ILogger<ClaimServiceRepo>>().Object;

            // calling the constructor of the service by passing the parameters.
            _claimRepo = new ClaimServiceRepo(_context, logger, _mapper);

            // calling the seeddata method where the context entities are assigned to the _context before teting.
            SeedData();
        }

        private void SeedData()
        {
            var policy = new Policy()
            {
                PolicyId = 1,
                PolicyNumber = "POL1001",
                PolicyType = "Health",
                PremiumAmount = 5000,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddYears(1),
                Status = "Active",
                CustomerId = 1
            };

            _context.Policies.Add(policy);

            _context.Claims.Add(new Claim()
            {
                ClaimId = 1,
                ClaimNumber = "CLM1001",
                ClaimDate = DateTime.UtcNow.AddDays(-5),
                ClaimAmount = 3000,
                Status = "Pending",
                Description = "Hospitalization",
                PolicyId = 1
            });

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllClaims_ShouldReturnClaims()
        {
            // Act
            var result = await _claimRepo.GetAllClaims();

            // Assert
            Assert.NotEmpty(result); 
            Assert.Equal("CLM1001", result.First().ClaimNumber);
            Assert.Equal("Pending", result.First().Status);
        }

        [Fact]
        public async Task GetClaimByClaimNumber_ShouldReturnSingleClaim()
        {
            // Act
            var result = await _claimRepo.GetClaimByClaimNumber("CLM1001");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("CLM1001", result.ClaimNumber);
        }

        [Fact]
        public async Task GetClaimByClaimNumber_ShouldThrow_WhenNotFound()
        {
            var result = _claimRepo.GetClaimByClaimNumber("INVALID");
            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>result);
            Assert.Equal("Claim not found.", exception.Message);
            
        }

        [Fact]
        public async Task GetClaimsByPolicyNumber_ShouldReturnClaims()
        {
            // Act
            var result = await _claimRepo.GetClaimsByPolicyNumber("POL1001");

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal("CLM1001", result.First().ClaimNumber);
        }

        [Fact]
        public async Task GetClaimsByPolicyNumber_ShouldThrow_WhenNotFound()
        {
            var result = _claimRepo.GetClaimsByPolicyNumber("POL9999");
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => result);
            Assert.Equal("No claims found.", exception?.Message);
        }

        [Fact]
        public async Task AddNewClaim_ShouldAddSuccessfully()
        {
            // Arrange
            var claimDto = new ClaimDto()
            {
                ClaimNumber = "CLM2002",
                ClaimDate = DateTime.UtcNow,
                ClaimAmount = 4000,
                Status = "Pending",
                PolicyId = 1,
                Description = "Accident Claim"
            };

            // Act
            var result = await _claimRepo.AddNewClaim(claimDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("CLM2002", result.ClaimNumber);
        }

        [Fact]
        public async Task AddNewClaim_ShouldThrow_WhenDuplicate()
        {
            var claimDto = new ClaimDto()
            {
                ClaimNumber = "CLM1001",
                ClaimDate = DateTime.UtcNow,
                ClaimAmount = 4000,
                Status = "Pending",
                PolicyId = 1
            };

            var result = _claimRepo.AddNewClaim(claimDto);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => result);
            Assert.Equal("Claim Already Exist", exception.Message);
        }

        [Fact]
        public async Task UpdateClaim_ShouldUpdateSuccessfully()
        {
            // Arrange
            var updatedClaim = new ClaimDto()
            {
                ClaimNumber = "CLM1001",
                ClaimDate = DateTime.UtcNow,
                ClaimAmount = 3500,
                Status = "Approved",
                PolicyId = 1,
                Description = "Updated"
            };

            // Act
            var result = await _claimRepo.UpdateClaim("CLM1001", updatedClaim);

            // Assert
            Assert.Equal("Approved", result.Status);
            Assert.Equal(3500, result.ClaimAmount);
        }

        [Fact]
        public async Task UpdateClaim_ShouldThrow_WhenNotFound()
        {
            var updatedClaim = new ClaimDto()
            {
                ClaimNumber = "INVALID",
                ClaimDate = DateTime.UtcNow,
                ClaimAmount = 3500,
                Status = "Approved",
                PolicyId = 1
            };

            var result = _claimRepo.UpdateClaim("INVALID", updatedClaim);
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => result);
            Assert.Equal("Claim not found.", exception.Message);
        }

        [Fact]
        public async Task DeleteClaim_ShouldDeleteSuccessfully()
        {
            // Act
            var result = await _claimRepo.DeleteClaim("CLM1001");

            // Assert
            Assert.True(result);
            Assert.Empty(_context.Claims.Where(x => x.ClaimNumber == "CLM1001"));
        }

        [Fact]
        public async Task DeleteClaim_ShouldThrow_WhenNotFound()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _claimRepo.DeleteClaim("INVALID"));
        }
    }
}
