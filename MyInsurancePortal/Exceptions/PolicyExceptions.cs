namespace MyInsurancePortal.Exceptions
{
    public class PolicyExceptions
    {
        public const string PolicyNotFound = "Policy not found.";
        public const string NoPoliciesFound = "No policies found.";
        public const string PolicyNumberRequired = "Policy number is required";
        public const string PolicyNumberMaxLength = "Policy number cannot exceed 50 characters";
        public const string PolicyTypeRequired = "Policy type is required";
        public const string PolicyTypeMaxLength = "Policy type cannot exceed 100 characters";
        public const string PremiumAmountRequired = "Premium amount is required";
        public const string PremiumAmountRange = "Premium amount must be greater than zero";
        public const string StartDateRequired = "Start date is required";
        public const string EndDateRequired = "End date is required";
        public const string StatusRequired = "Status is required";
        public const string StatusMaxLength = "Status cannot exceed 50 characters";
        public const string CustomerIdRequired = "Customer ID is required";
    }
}
