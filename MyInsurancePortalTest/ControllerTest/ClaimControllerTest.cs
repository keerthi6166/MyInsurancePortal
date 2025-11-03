using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MyInsurancePortal.Controllers;
using MyInsurancePortal.Interfaces;
using MyInsurancePortal.DtoModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace MyMyInsurancePortal.Tests.Controllers
{
    public class ClaimControllerTests
    {
        private readonly Mock<IClaimService> _mockService;
        private readonly ClaimController _controller;

        public ClaimControllerTests()
        {
            _mockService = new Mock<IClaimService>();
            _controller = new ClaimController(_mockService.Object, null); // context not used, so null
        }

        #region GetAllClaims

        [Fact]
        public async Task GetAllClaims_ShouldReturnOk_WithListOfClaims()
        {
            // Arrange
            var claims = new List<ClaimDto>
            {
                new() { ClaimNumber = "CLM001", ClaimDate = DateTime.Today, ClaimAmount = 5000, Status = "Approved", PolicyId = 1 },
                new() { ClaimNumber = "CLM002", ClaimDate = DateTime.Today, ClaimAmount = 2000, Status = "Pending", PolicyId = 1 }
            };
            _mockService.Setup(s => s.GetAllClaims()).ReturnsAsync(claims);

            // Act
            var result = await _controller.GetAllClaims();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<List<ClaimDto>>().Subject;
            returnValue.Should().HaveCount(2);
            _mockService.Verify(s => s.GetAllClaims(), Times.Once);
        }

        [Fact]
        public async Task GetAllClaims_ShouldThrowException_WhenServiceFails()
        {
            _mockService.Setup(s => s.GetAllClaims())
                        .ThrowsAsync(new Exception("Database down"));

            Func<Task> act = async () => await _controller.GetAllClaims();

            await act.Should().ThrowAsync<Exception>().WithMessage("Database down");
        }

        #endregion

        #region GetClaimById

        [Fact]
        public async Task GetClaimById_ShouldReturnOk_WhenClaimExists()
        {
            var claim = new ClaimDto
            {
                ClaimNumber = "CLM001",
                ClaimDate = DateTime.Today,
                ClaimAmount = 10000,
                Status = "Approved",
                PolicyId = 1
            };
            _mockService.Setup(s => s.GetClaimByClaimNumber("CLM001")).ReturnsAsync(claim);

            var result = await _controller.GetClaimById("CLM001");

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var dto = okResult.Value.Should().BeOfType<ClaimDto>().Subject;
            dto.ClaimNumber.Should().Be("CLM001");

            _mockService.Verify(s => s.GetClaimByClaimNumber("CLM001"), Times.Once);
        }

        [Fact]
        public async Task GetClaimById_ShouldThrowException_WhenNotFound()
        {
            _mockService.Setup(s => s.GetClaimByClaimNumber("INVALID"))
                        .ThrowsAsync(new KeyNotFoundException("Claim not found"));

            Func<Task> act = async () => await _controller.GetClaimById("INVALID");

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Claim not found");
        }

        #endregion

        #region AddNewClaim

        [Fact]
        public async Task AddNewClaim_ShouldReturnOk_WhenAddedSuccessfully()
        {
            var inputClaim = new ClaimDto
            {
                ClaimNumber = "CLM003",
                ClaimDate = DateTime.Today,
                ClaimAmount = 8000,
                Status = "Pending",
                PolicyId = 2
            };
            _mockService.Setup(s => s.AddNewClaim(It.IsAny<ClaimDto>()))
                        .ReturnsAsync(inputClaim);

            var result = await _controller.AddNewClaim(inputClaim);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var dto = okResult.Value.Should().BeOfType<ClaimDto>().Subject;
            dto.ClaimNumber.Should().Be("CLM003");

            _mockService.Verify(s => s.AddNewClaim(It.IsAny<ClaimDto>()), Times.Once);
        }

        [Fact]
        public async Task AddNewClaim_ShouldThrowException_WhenInvalidData()
        {
            _mockService.Setup(s => s.AddNewClaim(It.IsAny<ClaimDto>()))
                        .ThrowsAsync(new ArgumentException("Invalid Claim Data"));

            Func<Task> act = async () => await _controller.AddNewClaim(new ClaimDto());

            await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid Claim Data");
        }

        #endregion

        #region UpdateClaim

        [Fact]
        public async Task UpdateClaim_ShouldReturnOk_WhenUpdatedSuccessfully()
        {
            var updatedClaim = new ClaimDto
            {
                ClaimNumber = "CLM001",
                ClaimDate = DateTime.Today,
                ClaimAmount = 9999,
                Status = "Approved",
                PolicyId = 1
            };

            _mockService.Setup(s => s.UpdateClaim("CLM001", It.IsAny<ClaimDto>()))
                        .ReturnsAsync(updatedClaim);

            var result = await _controller.UpdateClaim("CLM001", updatedClaim);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var dto = okResult.Value.Should().BeOfType<ClaimDto>().Subject;
            dto.ClaimAmount.Should().Be(9999);

            _mockService.Verify(s => s.UpdateClaim("CLM001", It.IsAny<ClaimDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateClaim_ShouldThrowException_WhenClaimNotFound()
        {
            _mockService.Setup(s => s.UpdateClaim("CLM404", It.IsAny<ClaimDto>()))
                        .ThrowsAsync(new KeyNotFoundException("Claim not found"));

            Func<Task> act = async () => await _controller.UpdateClaim("CLM404", new ClaimDto());

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Claim not found");
        }

        #endregion

        #region DeleteClaim

        [Fact]
        public async Task DeleteClaim_ShouldReturnOk_WhenDeletedSuccessfully()
        {
            _mockService.Setup(s => s.DeleteClaim("CLM001")).ReturnsAsync(true);

            var result = await _controller.DeleteClaim("CLM001");

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);

            _mockService.Verify(s => s.DeleteClaim("CLM001"), Times.Once);
        }

        [Fact]
        public async Task DeleteClaim_ShouldThrowException_WhenClaimNotFound()
        {
            _mockService.Setup(s => s.DeleteClaim("CLM404"))
                        .ThrowsAsync(new KeyNotFoundException("Claim not found"));

            Func<Task> act = async () => await _controller.DeleteClaim("CLM404");

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Claim not found");
        }

        #endregion

        #region GetClaimsByPolicyId

        [Fact]
        public async Task GetClaimsByPolicyId_ShouldReturnOk_WithClaimsList()
        {
            var claims = new List<ClaimDto>
            {
                new() { ClaimNumber = "CLM777", ClaimDate = DateTime.Today, ClaimAmount = 12000, Status = "Pending", PolicyId = 3 }
            };
            _mockService.Setup(s => s.GetClaimsByPolicyNumber("POL123")).ReturnsAsync(claims);

            var result = await _controller.GetClaimsByPolicyId("POL123");

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var dtoList = okResult.Value.Should().BeAssignableTo<List<ClaimDto>>().Subject;
            dtoList.Should().ContainSingle();

            _mockService.Verify(s => s.GetClaimsByPolicyNumber("POL123"), Times.Once);
        }

        #endregion
    }
}
