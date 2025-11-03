namespace MyInsurancePortal.Exceptions
{
    public class CustomerExceptions
    {
        public const string CustomerNotFound = "Customer Not Found";
        public const string FullNameRequired = "Full name is required";
        public const string FullNameMaxLength = "Full name cannot exceed 100 characters";
        public const string EmailRequired = "Email is required";
        public const string EmailMaxLength = "Email cannot exceed 100 characters";
        public const string InvalidEmailFormat = "Invalid email format";
        public const string PhoneMaxLength = "Phone number cannot exceed 15 characters";
        public const string AddressMaxLength = "Address cannot exceed 200 characters";
        public const string DobRequired = "Date of Birth is required";

    }
}
