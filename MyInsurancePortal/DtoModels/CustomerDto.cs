using System.ComponentModel.DataAnnotations;
using MyInsurancePortal.Exceptions;

namespace MyInsurancePortal.DtoModels
{
    public class CustomerDto
    {
        [Required(ErrorMessage = CustomerExceptions.FullNameRequired)]
        [MaxLength(100, ErrorMessage = CustomerExceptions.FullNameMaxLength)]
        public string? FullName { get; set; }

        [Required(ErrorMessage = CustomerExceptions.EmailRequired)]
        [EmailAddress(ErrorMessage = CustomerExceptions.InvalidEmailFormat)]
        [MaxLength(100, ErrorMessage = CustomerExceptions.EmailMaxLength)]
        public string? Email { get; set; }

        [MaxLength(15, ErrorMessage = CustomerExceptions.PhoneMaxLength)]
        public string? PhoneNumber { get; set; }

        [MaxLength(200, ErrorMessage = CustomerExceptions.AddressMaxLength)]
        public string? Address { get; set; }

        [Required(ErrorMessage = CustomerExceptions.DobRequired)]
        public DateTime DateOfBirth { get; set; }
    }
}
