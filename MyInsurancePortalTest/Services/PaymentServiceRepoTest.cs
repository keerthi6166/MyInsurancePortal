using AutoMapper;
using MyInsurancePortal.DtoModels;
using MyInsurancePortal.Models;
using MyInsurancePortal.Repository;
using MyInsurancePortalTest.TestHelper;

namespace MyInsurancePortalTest.Services
{
    public class PaymentServiceRepoTest
    {
        private readonly InsuranceDbContext _context;
        private readonly IMapper _mapper;
        private readonly PaymentServiceRepo _paymentRepo;

        public PaymentServiceRepoTest()
        {
            _context = DbContextHelper.GetInMemoryDbContext();
            _mapper = MapperHelper.GetMapper();
            _paymentRepo = new PaymentServiceRepo(_context, _mapper);

        }

        private void SeeData()
        {
            _context.Policies.RemoveRange(_context.Policies);
            _context.Payments.RemoveRange(_context.Payments);
            _context.SaveChanges();

            var policy = new Policy
            {
                PolicyId = 1,
                PolicyNumber = "POL001",
                PolicyType = "Health",
                PremiumAmount = 1200,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1),
                Status = "Active",
                CustomerId = 1
            };

            var payments = new List<Payment>
            {
                new Payment
                {
                    PaymentId = 1,
                    PaymentDate = DateTime.Now,
                    AmountPaid = 500,
                    PaymentMode = "Credit Card",
                    TransactionId = "TXN001",
                    PolicyId = 1,
                    Policy = policy
                },
                new Payment
                {
                    PaymentId = 2,
                    PaymentDate = DateTime.Now,
                    AmountPaid = 700,
                    PaymentMode = "UPI",
                    TransactionId = "TXN002",
                    PolicyId = 1,
                    Policy = policy
                }
            };
            
            _context.Policies.Add(policy);
            _context.Payments.AddRange(payments);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllPayments_ShouldReturnList_WhenPaymentsExist()
        {
            SeeData();
            var result = await _paymentRepo.GetAllPayments();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllPayments_ShouldThrow_WhenNoPaymentsExist()
        {
            _context.Payments.RemoveRange(_context.Payments);
            await _context.SaveChangesAsync();

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _paymentRepo.GetAllPayments());
            Assert.Equal("No Payments found.", exception.Message);
        }

        [Fact]
        public async Task GetPaymentByTransactionId_ShouldReturnPayment_WhenFound()
        {
            SeeData();

            var result = await _paymentRepo.GetPaymentByTransactionId("TXN001");

            Assert.NotNull(result);
            Assert.Equal("Credit Card", result.PaymentMode);
        }

        [Fact]
        public async Task GetPaymentByTransactionId_ShouldThrow_WhenNotFound()
        {
            SeeData();

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _paymentRepo.GetPaymentByTransactionId("INVALID"));
            Assert.Equal("Payment not found.", exception.Message);
        }

        [Fact]
        public async Task GetPaymentsByPolicyNumber_ShouldReturnPayments_WhenFound()
        {
            SeeData();

            var result = await _paymentRepo.GetPaymentsByPolicyNumber("POL001");

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task AddNewPayment_ShouldAddPaymentSuccessfully()
        {
            SeeData();

            var dto = new PaymentDto
            {
                PaymentDate = DateTime.Now,
                AmountPaid = 1000,
                PaymentMode = "NetBanking",
                TransactionId = "TXN003",
                PolicyId = 1
            };

            var result = await _paymentRepo.AddNewPayment(dto);

            Assert.NotNull(result);
            Assert.Equal("TXN003", result.TransactionId);
            Assert.Equal(3, _context.Payments.Count());
        }

        [Fact]
        public async Task UpdatePayment_ShouldUpdateSuccessfully()
        {
            SeeData();

            var dto = new PaymentDto
            {
                TransactionId = "TXN001",
                PaymentDate = DateTime.Now,
                AmountPaid = 999,
                PaymentMode = "Debit Card",
                PolicyId = 1
            };

            var result = await _paymentRepo.UpdatePayment(dto);

            Assert.NotNull(result);
            Assert.Equal(999, result.AmountPaid);
            Assert.Equal("Debit Card", result.PaymentMode);
        }

        [Fact]
        public async Task UpdatePayment_ShouldThrow_WhenNotFound()
        {
            SeeData();

            var dto = new PaymentDto
            {
                TransactionId = "INVALID",
                PaymentDate = DateTime.Now,
                AmountPaid = 1000,
                PaymentMode = "UPI",
                PolicyId = 1
            };

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _paymentRepo.UpdatePayment(dto));
            Assert.Equal("Payment not found.", exception.Message);
        }

        [Fact]
        public async Task DeletePayment_ShouldDeleteSuccessfully()
        {
            SeeData();

            var result = await _paymentRepo.DeletePayment("TXN001");

            Assert.True(result);
            Assert.False(_context.Payments.Any(p => p.TransactionId == "TXN001"));
        }

        [Fact]
        public async Task DeletePayment_ShouldThrow_WhenNotFound()
        {
            SeeData();

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _paymentRepo.DeletePayment("invalid"));
            Assert.Equal("Payment not found.", exception.Message);

        }
    }
}
