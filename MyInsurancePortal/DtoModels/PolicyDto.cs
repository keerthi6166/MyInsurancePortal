using System.ComponentModel.DataAnnotations;
using MyInsurancePortal.Exceptions;

namespace MyInsurancePortal.DtoModels
{
    public class PolicyDto
    {
        [Required(ErrorMessage = PolicyExceptions.PolicyNumberRequired)]
        [MaxLength(50, ErrorMessage = PolicyExceptions.PolicyNumberMaxLength)]
        public string? PolicyNumber { get; set; }

        [Required(ErrorMessage = PolicyExceptions.PolicyTypeRequired)]
        [MaxLength(100, ErrorMessage = PolicyExceptions.PolicyTypeMaxLength)]
        public string? PolicyType { get; set; } // Health, Vehicle, Life, etc.

        [Required(ErrorMessage = PolicyExceptions.PremiumAmountRequired)]
        [Range(1, double.MaxValue, ErrorMessage = PolicyExceptions.PremiumAmountRange)]
        public decimal PremiumAmount { get; set; }

        [Required(ErrorMessage = PolicyExceptions.StartDateRequired)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = PolicyExceptions.EndDateRequired)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = PolicyExceptions.StatusRequired)]
        [MaxLength(50, ErrorMessage = PolicyExceptions.StatusMaxLength)]
        public string Status { get; set; } = "Active";

        [Required(ErrorMessage = PolicyExceptions.CustomerIdRequired)]
        public int CustomerId { get; set; }
    }
}
