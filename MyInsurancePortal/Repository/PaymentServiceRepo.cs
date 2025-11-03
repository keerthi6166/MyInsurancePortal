using MyInsurancePortal.Interfaces;
using MyInsurancePortal.Models;
using MyInsurancePortal.DtoModels;
using Microsoft.EntityFrameworkCore;
using MyInsurancePortal.Exceptions;
using AutoMapper;

namespace MyInsurancePortal.Repository
{
    public class PaymentServiceRepo : IPaymentService
    {
        private readonly InsuranceDbContext _context;
        private readonly IMapper _mapper;
        public PaymentServiceRepo(InsuranceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PaymentDto>> GetAllPayments()
        {
            var payments = await _context.Payments.ToListAsync();
            if (payments == null || payments.Count == 0)
            {
                throw new KeyNotFoundException(PaymentExceptions.NoPaymentsFound);
            }
            return _mapper.Map<List<PaymentDto>>(payments);
        }

        public async Task<PaymentDto?> GetPaymentByTransactionId(string transactionId)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(x=>x.TransactionId == transactionId);
            if (payment == null)
            {
                throw new KeyNotFoundException(PaymentExceptions.PaymentNotFound);
            }
            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<List<PaymentDto>> GetPaymentsByPolicyNumber(string policyNumber)
        {

            var payments = await _context.Payments
                .Include(x=>x.Policy)
                .Where(x => x.Policy.PolicyNumber == policyNumber).ToListAsync();

            if (payments == null)
            {
                throw new KeyNotFoundException(PaymentExceptions.NoPaymentsFound);

            }
            return _mapper.Map<List<PaymentDto>>(payments);
        }

        public async Task<PaymentDto> AddNewPayment(PaymentDto paymentDto)
        {
            var payment = _mapper.Map<Payment>(paymentDto);
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<PaymentDto> UpdatePayment(PaymentDto paymentDto)
        {
            var existingPayment = await _context.Payments.FirstOrDefaultAsync(x=>x.TransactionId == paymentDto.TransactionId);
            if (existingPayment == null)
            {
                throw new KeyNotFoundException(PaymentExceptions.PaymentNotFound); 
            }
            _mapper.Map(paymentDto, existingPayment);
            await _context.SaveChangesAsync();

            return _mapper.Map<PaymentDto>(existingPayment);
        }

        public async Task<bool> DeletePayment(string transactionId)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(x=>x.TransactionId == transactionId);
            if (payment == null)
            {
                throw new KeyNotFoundException(PaymentExceptions.PaymentNotFound);
            }
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;

        }
    
    }

}
