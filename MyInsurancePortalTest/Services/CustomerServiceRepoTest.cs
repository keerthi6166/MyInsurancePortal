using AutoMapper;
using MyInsurancePortal.DtoModels;
using MyInsurancePortal.Models;
using MyInsurancePortal.Repository;
using MyInsurancePortalTest.TestHelper;
using Microsoft.Extensions.Logging;
using Moq;

namespace MyInsurancePortalTest.Services
{
    public class CustomerServiceRepoTest
    {
        private readonly InsuranceDbContext _context;
        private readonly IMapper _mapper;
        private readonly CustomerServiceRepo _customerRepo;
        public CustomerServiceRepoTest() 
        { 
            _context = DbContextHelper.GetInMemoryDbContext();
            _mapper = MapperHelper.GetMapper();

            var logger = new Mock<ILogger<CustomerServiceRepo>>().Object;

            _customerRepo = new CustomerServiceRepo(_context, logger, _mapper);

            SeeData();

        }

        private void SeeData()
        {
            var customer = new Customer()
            {
                CustomerId = 1,
                FullName = "keerthivasan selvam",
                Email = "keerthivasanselvam@gmail.com",
                PhoneNumber = "1234567890",
                Address = "Sathy",
                DateOfBirth = DateTime.Now,
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllCustomers_ShouldReturncustomers()
        {
            var result  = await _customerRepo.GetAllCustomers(); //here we await the task of storing the value to the result and pauses until it get completed, but the thread will be free.

            Assert.Equal("keerthivasanselvam@gmail.com", result.First().Email);
            Assert.Equal("1234567890", result.First().PhoneNumber);

        }

        [Fact]
        public async Task GetAllCustomers_ShouldThrowException()
        {
            var context = DbContextHelper.GetInMemoryDbContext(); // new blank DB
            var logger = new Mock<ILogger<CustomerServiceRepo>>().Object;
            var mapper = MapperHelper.GetMapper();
            var emptyRepo = new CustomerServiceRepo(context, logger, mapper);

            // await means “Pause this method until the awaited Task finishes, then continue.”
            // dont use await here because the exceptio will be thrown before the result is ready.
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => emptyRepo.GetAllCustomers());
            Assert.Equal("Customer Not Found", exception.Message);
                
        }

        [Fact]
        public async Task GetCustomerByEmail_shouldReturnCustomer()
        {
            var result = await _customerRepo.GetCustomerByEmail("keerthivasanselvam@gmail.com");
            Assert.Equal("keerthivasanselvam@gmail.com", result.Email);
        }

        [Fact]
        public async Task GetCustomerByEmail_ShouldReturnException()
        {
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _customerRepo.GetCustomerByEmail("example@email.com"));
            Assert.Equal("Customer Not Found", exception.Message);
        }

        [Fact]
        public async Task AddNewCustomer_ShouldReturnCustomer()
        {
            var customer = new CustomerDto()
            {
                FullName = "Alamelu Selvam",
                Email = "alameluselvam@gmail.com",
                PhoneNumber = "9476543210",
                Address = "123, Anna Nagar, Chennai, India",
                DateOfBirth = new DateTime(1995, 6, 15)
            };
            var result = await _customerRepo.AddNewCustomer(customer);
            Assert.Equal("alameluselvam@gmail.com", result.Email);
        }

        [Fact]
        public async Task UpdateCutomer_ShouldUpdateSuccessfully()
        {
            var customer = new CustomerDto()
            {
                FullName = "keerthivasan selvam",
                Email = "keerthivasanselvam@gmail.com",
                PhoneNumber = "1234567890",
                Address = "Sathyamangalam",
                DateOfBirth = DateTime.Now,
            };

            var result = await _customerRepo.UpdateCustomer(customer);
            Assert.Equal("Sathyamangalam", result.Address);
        }

        [Fact]
        public async Task UpdateCustomer_ShouldThrowException()
        {
            var customer = new CustomerDto()
            {
                FullName = "Alamelu Selvam",
                Email = "alameluselvam@gmail.com",
                PhoneNumber = "9476543210",
                Address = "123, Anna Nagar, Chennai, India",
                DateOfBirth = new DateTime(1995, 6, 15)
            };

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(()=> _customerRepo.UpdateCustomer(customer));
            Assert.Equal("Customer Not Found", exception.Message);
        }

        [Fact]
        public async Task DeleteCustomer_ShouldDeleteSuccessfully()
        {
            // Act
            var result = await _customerRepo.DeleteCustomer("keerthivasanselvam@gmail.com");

            // Assert
            Assert.True(result);
            Assert.Empty(_context.Customers.Where(x => x.Email == "keerthivasanselvam@gmail.com"));
        }

        [Fact]
        public async Task DeleteCustomer_ShouldThrow_WhenNotFound()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _customerRepo.DeleteCustomer("vasanselvam@gmail.com"));
        }
    }
}

