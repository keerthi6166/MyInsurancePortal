namespace MyInsurancePortal.Exceptions
{
    public class ClaimExceptions
    {
        public const string ClaimNotFound = "Claim not found.";
        public const string NoClaimsFound = "No claims found.";
        public const string ClaimAlreadyExist = "Claim Already Exist";
        public const string ClaimNumberRequired = "Claim Number is required";
        public const string ClaimNumberMaxLength = "Claim Number cannot exceed 50 characters";
        public const string ClaimDateRequired = "Claim Date is required";
        public const string ClaimAmountRequired = "Claim Amount is required";
        public const string ClaimAmountRange = "Claim Amount must be greater than zero";
        public const string StatusRequired = "Status is required";
        public const string StatusMaxLength = "Status cannot exceed 50 characters";
        public const string DescriptionMaxLength = "Description cannot exceed 500 characters";
        public const string PolicyIdRequired = "Policy ID is required";
    }

}

