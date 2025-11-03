using System.ComponentModel.DataAnnotations;
using MyInsurancePortal.Exceptions;

namespace MyInsurancePortal.DtoModels
{
    public class PaymentDto
    {
        [Required(ErrorMessage = PaymentExceptions.PaymentDateRequired)]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = PaymentExceptions.AmountPaidRequired)]
        [Range(1, double.MaxValue, ErrorMessage = PaymentExceptions.AmountPaidRange)]
        public decimal AmountPaid { get; set; }

        [Required(ErrorMessage = PaymentExceptions.PaymentModeRequired)]
        [MaxLength(50, ErrorMessage = PaymentExceptions.PaymentModeMaxLength)]
        public string? PaymentMode { get; set; }

        [Required(ErrorMessage = PaymentExceptions.TransactionIdRequired)]
        [MaxLength(20, ErrorMessage = PaymentExceptions.TransactionIdMaxLength)]
        public string? TransactionId { get; set; }

        [Required(ErrorMessage = PaymentExceptions.PolicyIdRequired)]
        public int PolicyId { get; set; }
    }
}
