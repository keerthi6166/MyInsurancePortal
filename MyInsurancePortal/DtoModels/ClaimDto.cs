using System;
using MyInsurancePortal.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace MyInsurancePortal.DtoModels
{
    public class ClaimDto
    {
        [Required(ErrorMessage = ClaimExceptions.ClaimNumberRequired)]
        [MaxLength(50)]
        public string ClaimNumber { get; set; }

        [Required(ErrorMessage = ClaimExceptions.ClaimDateRequired)]
        public DateTime ClaimDate { get; set; }

        [Required(ErrorMessage = ClaimExceptions.ClaimAmountRequired)]
        [Range(1, double.MaxValue, ErrorMessage = ClaimExceptions.ClaimAmountRange)]
        public decimal ClaimAmount { get; set; }

        [Required(ErrorMessage = ClaimExceptions.StatusRequired)]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        [MaxLength(500, ErrorMessage = ClaimExceptions.DescriptionMaxLength)]
        public string? Description { get; set; }

        [Required(ErrorMessage = ClaimExceptions.PolicyIdRequired)]
        public int PolicyId { get; set; }
    }
}
