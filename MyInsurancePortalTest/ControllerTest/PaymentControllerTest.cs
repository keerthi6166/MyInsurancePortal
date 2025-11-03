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
    public class PaymentControllerTest
    {
        private readonly Mock<IPaymentService> _mockService;
        private readonly PaymentController _controller;

        public PaymentControllerTest()
        {
            _mockService = new Mock<IPaymentService>();
            _controller = new PaymentController(_mockService.Object);
        }

        #region GetAllPayments

        [Fact]
        public async Task GetAllPayments_ShouldReturnOk_WithListOfPayments()
        {
            // Arrange
            var payments = new List<PaymentDto>
            {
                new PaymentDto
                {
                    PaymentDate = DateTime.UtcNow.AddDays(-5),
                    AmountPaid = 5000,
                    PaymentMode = "Credit Card",
                    TransactionId = "TXN001",
                    PolicyId = 101
                },
                new PaymentDto
                {
                    PaymentDate = DateTime.UtcNow.AddDays(-2),
                    AmountPaid = 2000,
                    PaymentMode = "UPI",
                    TransactionId = "TXN002",
                    PolicyId = 102
                }
            };

            _mockService.Setup(s => s.GetAllPayments()).ReturnsAsync(payments);

            // Act
            var result = await _controller.GetallPayments();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var value = okResult.Value.Should().BeAssignableTo<List<PaymentDto>>().Subject;
            value.Should().HaveCount(2);
            _mockService.Verify(s => s.GetAllPayments(), Times.Once);
        }

        [Fact]
        public async Task GetAllPayments_ShouldThrowException_WhenServiceFails()
        {
            _mockService.Setup(s => s.GetAllPayments())
                        .ThrowsAsync(new Exception("Database down"));

            Func<Task> act = async () => await _controller.GetallPayments();

            await act.Should().ThrowAsync<Exception>().WithMessage("Database down");
        }

        #endregion

        #region GetPaymentByTransactionId

        [Fact]
        public async Task GetPaymentByTransactionId_ShouldReturnOk_WhenPaymentExists()
        {
            var payment = new PaymentDto
            {
                PaymentDate = DateTime.UtcNow,
                AmountPaid = 3000,
                PaymentMode = "Debit Card",
                TransactionId = "TXN123",
                PolicyId = 105
            };

            _mockService.Setup(s => s.GetPaymentByTransactionId("TXN123"))
                        .ReturnsAsync(payment);

            var result = await _controller.GetPaymentByTransactionId("TXN123");

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var value = okResult.Value.Should().BeOfType<PaymentDto>().Subject;

            value.TransactionId.Should().Be("TXN123");
            _mockService.Verify(s => s.GetPaymentByTransactionId("TXN123"), Times.Once);
        }

        [Fact]
        public async Task GetPaymentByTransactionId_ShouldThrowException_WhenNotFound()
        {
            _mockService.Setup(s => s.GetPaymentByTransactionId("INVALID"))
                        .ThrowsAsync(new KeyNotFoundException("Payment Not Found"));

            Func<Task> act = async () => await _controller.GetPaymentByTransactionId("INVALID");

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Payment Not Found");
        }

        #endregion

        #region GetPaymentsByPolicyNumber

        [Fact]
        public async Task GetPaymentsByPolicyNumber_ShouldReturnOk_WhenPaymentsExist()
        {
            var payments = new List<PaymentDto>
            {
                new PaymentDto
                {
                    PaymentDate = DateTime.UtcNow.AddDays(-1),
                    AmountPaid = 1500,
                    PaymentMode = "UPI",
                    TransactionId = "TXN005",
                    PolicyId = 10
                }
            };

            _mockService.Setup(s => s.GetPaymentsByPolicyNumber("POL123"))
                        .ReturnsAsync(payments);

            var result = await _controller.GetPaymentsByPolicyNumber("POL123");

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var value = okResult.Value.Should().BeAssignableTo<List<PaymentDto>>().Subject;
            value.Should().HaveCount(1);
            _mockService.Verify(s => s.GetPaymentsByPolicyNumber("POL123"), Times.Once);
        }

        [Fact]
        public async Task GetPaymentsByPolicyNumber_ShouldThrowException_WhenNotFound()
        {
            _mockService.Setup(s => s.GetPaymentsByPolicyNumber("INVALID"))
                        .ThrowsAsync(new KeyNotFoundException("No Payments Found"));

            Func<Task> act = async () => await _controller.GetPaymentsByPolicyNumber("INVALID");

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("No Payments Found");
        }

        #endregion

        #region AddNewPayment

        [Fact]
        public async Task AddNewPayment_ShouldReturnOk_WhenAddedSuccessfully()
        {
            var payment = new PaymentDto
            {
                PaymentDate = DateTime.UtcNow,
                AmountPaid = 2500,
                PaymentMode = "NetBanking",
                TransactionId = "TXN200",
                PolicyId = 1001
            };

            _mockService.Setup(s => s.AddNewPayment(It.IsAny<PaymentDto>()))
                        .ReturnsAsync(payment);

            var result = await _controller.AddNewPayment(payment);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var value = okResult.Value.Should().BeOfType<PaymentDto>().Subject;

            value.TransactionId.Should().Be("TXN200");
            _mockService.Verify(s => s.AddNewPayment(It.IsAny<PaymentDto>()), Times.Once);
        }

        #endregion

        #region UpdatePayment

        [Fact]
        public async Task UpdatePayment_ShouldReturnOk_WhenPaymentExists()
        {
            var payment = new PaymentDto
            {
                PaymentDate = DateTime.UtcNow,
                AmountPaid = 5000,
                PaymentMode = "Credit Card",
                TransactionId = "TXN888",
                PolicyId = 20
            };

            _mockService.Setup(s => s.UpdatePayment(It.IsAny<PaymentDto>()))
                        .ReturnsAsync(payment);

            var result = await _controller.UpdatePayment(payment);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var value = okResult.Value.Should().BeOfType<PaymentDto>().Subject;
            _mockService.Verify(s => s.UpdatePayment(It.IsAny<PaymentDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePayment_ShouldThrowException_WhenNotFound()
        {
            _mockService.Setup(s => s.UpdatePayment(It.IsAny<PaymentDto>()))
                        .ThrowsAsync(new KeyNotFoundException("Payment Not Found"));

            var payment = new PaymentDto
            {
                PaymentDate = DateTime.UtcNow,
                AmountPaid = 5000,
                PaymentMode = "Credit Card",
                TransactionId = "TXN999",
                PolicyId = 30
            };

            Func<Task> act = async () => await _controller.UpdatePayment(payment);

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Payment Not Found");
        }

        #endregion

        #region DeletePayment

        [Fact]
        public async Task DeletePayment_ShouldReturnTrue_WhenDeletedSuccessfully()
        {
            _mockService.Setup(s => s.DeletePayment("TXN001")).ReturnsAsync(true);

            var result = await _controller.DeletePayment("TXN001");

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);

            _mockService.Verify(s => s.DeletePayment("TXN001"), Times.Once);
        }

        [Fact]
        public async Task DeletePayment_ShouldThrowException_WhenPaymentNotFound()
        {
            _mockService.Setup(s => s.DeletePayment("INVALID"))
                        .ThrowsAsync(new KeyNotFoundException("Payment Not Found"));

            Func<Task> act = async () => await _controller.DeletePayment("INVALID");

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Payment Not Found");
        }

        #endregion
    }
}
 