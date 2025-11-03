using MyInsurancePortal.Models;
using MyInsurancePortal.DtoModels;

namespace MyInsurancePortal.Interfaces
{
    public interface IPaymentService
    {
        Task<List<PaymentDto>> GetAllPayments();
        Task<PaymentDto?> GetPaymentByTransactionId(string transactionId);
        Task<List<PaymentDto>> GetPaymentsByPolicyNumber(string policyNumber);
        Task<PaymentDto> AddNewPayment(PaymentDto payment);
        Task<PaymentDto> UpdatePayment(PaymentDto payment);
        Task<bool> DeletePayment(string transactionId);
    }
}
