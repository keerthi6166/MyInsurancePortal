using FluentAssertions;
using MyInsurancePortal.Controllers;
using MyInsurancePortal.DtoModels;
using MyInsurancePortal.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MyMyInsurancePortalTest.ControllerTests
{
    public class CustomerControllerTest
    {
        private readonly Mock<ICustomerService> _mockService;
        private readonly CustomerController _controller;

        public CustomerControllerTest()
        {
            _mockService = new Mock<ICustomerService>();
            _controller = new CustomerController(_mockService.Object);
        }

        #region GetAllCustomers

        [Fact]
        public async Task GetAllCustomers_ShouldReturnOk_WithListOfCustomers()
        {
            // Arrange
            var customers = new List<CustomerDto>
            {
                new CustomerDto { FullName = "Keerthivasan", Email = "keerthivasan.s@example.com", PhoneNumber = "9876543210", Address = "Chennai", DateOfBirth = new DateTime(1998,5,15) },
                new CustomerDto { FullName = "Manoj", Email = "manoj.kumar@example.com", PhoneNumber = "9876501234", Address = "Bangalore", DateOfBirth = new DateTime(1995,10,20) }
            };

            _mockService.Setup(s => s.GetAllCustomers()).ReturnsAsync(customers);

            // Act
            var result = await _controller.GetAllCustomers();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<List<CustomerDto>>().Subject;
            returnValue.Should().HaveCount(2);
            _mockService.Verify(s => s.GetAllCustomers(), Times.Once);
        }

        [Fact]
        public async Task GetAllCustomers_ShouldThrowException_WhenServiceFails()
        {
            _mockService.Setup(s => s.GetAllCustomers())
                .ThrowsAsync(new Exception("Database down"));

            Func<Task> act = async () => await _controller.GetAllCustomers();

            await act.Should().ThrowAsync<Exception>().WithMessage("Database down");
        }

        #endregion

        #region GetCustomerByEmail

        [Fact]
        public async Task GetCustomerByEmail_ShouldReturnOk_WhenCustomerExists()
        {
            var customer = new CustomerDto
            {
                FullName = "Keerthivasan",
                Email = "keerthivasan.s@example.com",
                PhoneNumber = "9876543210",
                Address = "Chennai",
                DateOfBirth = new DateTime(1998, 5, 15)
            };

            _mockService.Setup(s => s.GetCustomerByEmail("keerthivasan.s@example.com")).ReturnsAsync(customer);

            var result = await _controller.GetCustomerById("keerthivasan.s@example.com");

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeOfType<CustomerDto>().Subject;

            _mockService.Verify(s => s.GetCustomerByEmail("keerthivasan.s@example.com"), Times.Once);
        }

        [Fact]
        public async Task GetCustomerByEmail_ShouldThrowException_WhenNotFound()
        {
            _mockService.Setup(s => s.GetCustomerByEmail("INVALID"))
                        .ThrowsAsync(new KeyNotFoundException("Customer Not Found"));

            Func<Task> act = async () => await _controller.GetCustomerById("INVALID");

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Customer Not Found");
        }

        #endregion

        #region AddNewCustomer

        [Fact]
        public async Task AddNewCustomer_ShouldReturnOk_WithCreatedCustomer()
        {
            var customer = new CustomerDto
            {
                FullName = "Keerthivasan",
                Email = "keerthivasan.s@example.com",
                PhoneNumber = "9876543210",
                Address = "Chennai",
                DateOfBirth = new DateTime(1998, 5, 15)
            };

            _mockService.Setup(s => s.AddNewCustomer(It.IsAny<CustomerDto>())).ReturnsAsync(customer);

            var result = await _controller.AddNewCustomer(customer);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeOfType<CustomerDto>().Subject;

            _mockService.Verify(s => s.AddNewCustomer(It.IsAny<CustomerDto>()), Times.Once);
        }

        #endregion

        #region UpdateCustomer

        [Fact]
        public async Task UpdateCustomer_ShouldReturnOk_WhenCustomerExists()
        {
            var customer = new CustomerDto
            {
                FullName = "Keerthivasan",
                Email = "keerthivasan.s@example.com",
                PhoneNumber = "9876543210",
                Address = "Chennai",
                DateOfBirth = new DateTime(1998, 5, 15)
            };

            _mockService.Setup(s => s.UpdateCustomer(It.IsAny<CustomerDto>())).ReturnsAsync(customer);

            var result = await _controller.UpdateCustomer(customer);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeOfType<CustomerDto>().Subject;

            _mockService.Verify(s => s.UpdateCustomer(It.IsAny<CustomerDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCustomer_ShouldThrowException_WhenNotFound()
        {
            _mockService.Setup(s => s.UpdateCustomer(It.IsAny<CustomerDto>()))
                        .ThrowsAsync(new KeyNotFoundException("Customer Not Found"));

            var customer = new CustomerDto
            {
                FullName = "Keerthivasan",
                Email = "keerthivasan.s@example.com",
                PhoneNumber = "9876543210",
                Address = "Chennai",
                DateOfBirth = new DateTime(1998, 5, 15)
            };

            Func<Task> act = async () => await _controller.UpdateCustomer(customer);

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Customer Not Found");
        }

        #endregion

        #region DeleteCustomer

        [Fact]
        public async Task DeleteCustomer_ShouldReturnTrue_WhenDeleted()
        {
            _mockService.Setup(s => s.DeleteCustomer("keerthivasan.s@example.com")).ReturnsAsync(true);

            var result = await _controller.DeleteCustomer("keerthivasan.s@example.com");

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);

            _mockService.Verify(s => s.DeleteCustomer("keerthivasan.s@example.com"), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomer_ShouldThrowException_WhenNotFound()
        {
            _mockService.Setup(s => s.DeleteCustomer("INVALID"))
                        .ThrowsAsync(new KeyNotFoundException("Customer Not Found"));

            Func<Task> act = async () => await _controller.DeleteCustomer("INVALID");

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Customer Not Found");
        }

        #endregion
    }
}
