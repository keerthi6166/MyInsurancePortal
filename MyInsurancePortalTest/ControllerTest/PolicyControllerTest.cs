using FluentAssertions;
using MyInsurancePortal.Controllers;
using MyInsurancePortal.DtoModels;
using MyInsurancePortal.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MyInsurancePortalTest.ControllerTests
{
    public class PolicyControllerTest
    {
        private readonly Mock<IPolicyService> _mockService;
        private readonly PolicyController _controller;

        public PolicyControllerTest()
        {
            _mockService = new Mock<IPolicyService>();
            _controller = new PolicyController(_mockService.Object);
        }

        #region GetAllPolicies

        [Fact]
        public async Task GetAllPolicies_ShouldReturnOk_WithListOfPolicies()
        {
            var policies = new List<PolicyDto>
            {
                new PolicyDto
                {
                    PolicyNumber = "POL001",
                    PolicyType = "Life",
                    PremiumAmount = 5000,
                    StartDate = DateTime.Now.AddYears(-1),
                    EndDate = DateTime.Now.AddYears(4),
                    Status = "Active",
                    CustomerId = 1
                },
                new PolicyDto
                {
                    PolicyNumber = "POL002",
                    PolicyType = "Health",
                    PremiumAmount = 3000,
                    StartDate = DateTime.Now.AddYears(-2),
                    EndDate = DateTime.Now.AddYears(3),
                    Status = "Active",
                    CustomerId = 2
                },
                new PolicyDto
                {
                    PolicyNumber = "POL003",
                    PolicyType = "Vehicle",
                    PremiumAmount = 2000,
                    StartDate = DateTime.Now.AddYears(-1),
                    EndDate = DateTime.Now.AddYears(2),
                    Status = "Expired",
                    CustomerId = 3
                }
            };

            _mockService.Setup(s => s.GetAllPolicies()).ReturnsAsync(policies);

            var result = await _controller.GetAllPolicies();

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var value = okResult.Value.Should().BeAssignableTo<List<PolicyDto>>().Subject;
            value.Should().HaveCount(3);

            _mockService.Verify(s => s.GetAllPolicies(), Times.Once);
        }

        [Fact]
        public async Task GetAllPolicies_ShouldThrowException_WhenServiceFails()
        {
            _mockService.Setup(s => s.GetAllPolicies())
                        .ThrowsAsync(new Exception("Database down"));

            Func<Task> act = async () => await _controller.GetAllPolicies();

            await act.Should().ThrowAsync<Exception>().WithMessage("Database down");
        }

        #endregion

        #region GetPolicyByPolicyNumber

        [Fact]
        public async Task GetPolicyByPolicyNumber_ShouldReturnOk_WhenPolicyExists()
        {
            var policy = new PolicyDto
            {
                PolicyNumber = "POL001",
                PolicyType = "Life",
                PremiumAmount = 5000,
                StartDate = DateTime.Now.AddYears(-1),
                EndDate = DateTime.Now.AddYears(4),
                Status = "Active",
                CustomerId = 1
            };

            _mockService.Setup(s => s.GetPolicyByPolicyNumber("POL001")).ReturnsAsync(policy);

            var result = await _controller.GetPolicyByPolicyNumber("POL001");

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var value = okResult.Value.Should().BeOfType<PolicyDto>().Subject;

            _mockService.Verify(s => s.GetPolicyByPolicyNumber("POL001"), Times.Once);
        }

        [Fact]
        public async Task GetPolicyByPolicyNumber_ShouldThrowException_WhenNotFound()
        {
            _mockService.Setup(s => s.GetPolicyByPolicyNumber("INVALID"))
                        .ThrowsAsync(new KeyNotFoundException("Policy Not Found"));

            Func<Task> act = async () => await _controller.GetPolicyByPolicyNumber("INVALID");

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Policy Not Found");
        }

        #endregion

        #region GetPoliciesByCustomerEmail

        [Fact]
        public async Task GetPoliciesByCustomerEmail_ShouldReturnOk_WhenPoliciesExist()
        {
            var policies = new List<PolicyDto>
            {
                new PolicyDto
                {
                    PolicyNumber = "POL001",
                    PolicyType = "Life",
                    PremiumAmount = 5000,
                    StartDate = DateTime.Now.AddYears(-1),
                    EndDate = DateTime.Now.AddYears(4),
                    Status = "Active",
                    CustomerId = 1
                },
                new PolicyDto
                {
                    PolicyNumber = "POL002",
                    PolicyType = "Health",
                    PremiumAmount = 3000,
                    StartDate = DateTime.Now.AddYears(-2),
                    EndDate = DateTime.Now.AddYears(3),
                    Status = "Active",
                    CustomerId = 1
                }
            };

            _mockService.Setup(s => s.GetPolicyByCustomerEmail("keerthivasan.s@example.com"))
                        .ReturnsAsync(policies);

            var result = await _controller.GetPoliciesByCustomerEmail("keerthivasan.s@example.com");

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var value = okResult.Value.Should().BeAssignableTo<List<PolicyDto>>().Subject;
            value.Should().HaveCount(2);

            _mockService.Verify(s => s.GetPolicyByCustomerEmail("keerthivasan.s@example.com"), Times.Once);
        }

        [Fact]
        public async Task GetPoliciesByCustomerEmail_ShouldThrowException_WhenServiceFails()
        {
            _mockService.Setup(s => s.GetPolicyByCustomerEmail("INVALID"))
                        .ThrowsAsync(new Exception("Customer not found"));

            Func<Task> act = async () => await _controller.GetPoliciesByCustomerEmail("INVALID");

            await act.Should().ThrowAsync<Exception>().WithMessage("Customer not found");
        }

        #endregion

        #region AddNewPolicy

        [Fact]
        public async Task AddNewPolicy_ShouldReturnOk_WithPolicyDetails()
        {
            var policy = new PolicyDto
            {
                PolicyNumber = "POL100",
                PolicyType = "Life",
                PremiumAmount = 4000,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(5),
                Status = "Active",
                CustomerId = 10
            };

            _mockService.Setup(s => s.AddNewPolicy(It.IsAny<PolicyDto>())).ReturnsAsync(policy);

            var result = await _controller.AddNewPolicy(policy);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var value = okResult.Value.Should().BeOfType<PolicyDto>().Subject;

            _mockService.Verify(s => s.AddNewPolicy(It.IsAny<PolicyDto>()), Times.Once);
        }

        #endregion

        #region UpdatePolicy

        [Fact]
        public async Task UpdatePolicy_ShouldReturnOk_WhenPolicyExists()
        {
            var policy = new PolicyDto
            {
                PolicyNumber = "POL001",
                PolicyType = "Health",
                PremiumAmount = 3500,
                StartDate = DateTime.Now.AddYears(-1),
                EndDate = DateTime.Now.AddYears(4),
                Status = "Active",
                CustomerId = 1
            };

            _mockService.Setup(s => s.UpdatePolicy(It.IsAny<PolicyDto>())).ReturnsAsync(policy);

            var result = await _controller.UpdatePolicy(policy);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var value = okResult.Value.Should().BeOfType<PolicyDto>().Subject;

            _mockService.Verify(s => s.UpdatePolicy(It.IsAny<PolicyDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePolicy_ShouldThrowException_WhenPolicyNotFound()
        {
            _mockService.Setup(s => s.UpdatePolicy(It.IsAny<PolicyDto>()))
                        .ThrowsAsync(new KeyNotFoundException("Policy Not Found"));

            var policy = new PolicyDto
            {
                PolicyNumber = "INVALID",
                PolicyType = "Health",
                PremiumAmount = 3000,
                StartDate = DateTime.Now.AddYears(-1),
                EndDate = DateTime.Now.AddYears(2),
                Status = "Active",
                CustomerId = 1
            };

            Func<Task> act = async () => await _controller.UpdatePolicy(policy);

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Policy Not Found");
        }

        #endregion

        #region DeletePolicy

        [Fact]
        public async Task DeletePolicy_ShouldReturnTrue_WhenDeletedSuccessfully()
        {
            _mockService.Setup(s => s.DeletePolicy("POL001")).ReturnsAsync(true);

            var result = await _controller.DeletePolicy("POL001");

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);

            _mockService.Verify(s => s.DeletePolicy("POL001"), Times.Once);
        }

        [Fact]
        public async Task DeletePolicy_ShouldThrowException_WhenPolicyNotFound()
        {
            _mockService.Setup(s => s.DeletePolicy("INVALID"))
                        .ThrowsAsync(new KeyNotFoundException("Policy Not Found"));

            Func<Task> act = async () => await _controller.DeletePolicy("INVALID");

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Policy Not Found");
        }

        #endregion
    }
}
