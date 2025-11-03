using MyInsurancePortal.Models;
using MyInsurancePortal.DtoModels;

namespace MyInsurancePortal.Interfaces
{
    public interface ICustomerService
    {
        
            Task<List<CustomerDto>> GetAllCustomers();
            Task<CustomerDto?> GetCustomerByEmail(string email);
            Task<CustomerDto> AddNewCustomer(CustomerDto customer);
            Task<CustomerDto> UpdateCustomer(CustomerDto customer);
            Task<bool> DeleteCustomer(string customerId);
        
    }
}
