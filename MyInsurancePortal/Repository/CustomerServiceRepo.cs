using AutoMapper;
using MyInsurancePortal.Exceptions;
using MyInsurancePortal.Interfaces;
using MyInsurancePortal.Models;
using MyInsurancePortal.DtoModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace MyInsurancePortal.Repository
{
    public class CustomerServiceRepo : ICustomerService
    {
        private readonly InsuranceDbContext _context;
        private readonly ILogger<CustomerServiceRepo> _logger;
        private readonly IMapper _mapper;

        public CustomerServiceRepo( InsuranceDbContext context, ILogger<CustomerServiceRepo> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<CustomerDto>> GetAllCustomers()
        {
            var customer =  await _context.Customers.ToListAsync();

            if (!customer.Any())
                throw new KeyNotFoundException(CustomerExceptions.CustomerNotFound);

            return _mapper.Map<List<CustomerDto>>(customer);
        }

        public async Task<CustomerDto?> GetCustomerByEmail(string email)
        {
            var customer = await _context.Customers.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (customer == null)
            {
                throw new KeyNotFoundException(CustomerExceptions.CustomerNotFound);
            }
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto> AddNewCustomer(CustomerDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return _mapper.Map<CustomerDto>(customer)   ;
        }

        public async Task<CustomerDto> UpdateCustomer(CustomerDto customerDto)
        {
            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Email == customerDto.Email);
            if (existingCustomer == null)
            {
                throw new KeyNotFoundException(CustomerExceptions.CustomerNotFound);
            }

            _mapper.Map(customerDto, existingCustomer);
            await _context.SaveChangesAsync();

            return _mapper.Map<CustomerDto>(existingCustomer);
        }


        public async Task<bool> DeleteCustomer(string email)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x=>x.Email == email);
            if(customer == null)
            {
                throw new KeyNotFoundException(CustomerExceptions.CustomerNotFound);
            }
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
