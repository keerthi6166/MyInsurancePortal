namespace MyInsurancePortal.Exceptions
{
    public class PaymentExceptions
    {
        public const string PaymentNotFound = "Payment not found.";
        public const string NoPaymentsFound = "No Payments found.";
        public const string PaymentDateRequired = "Payment date is required";
        public const string AmountPaidRequired = "Amount paid is required";
        public const string AmountPaidRange = "Amount paid must be greater than zero";
        public const string PaymentModeRequired = "Payment mode is required";
        public const string PaymentModeMaxLength = "Payment mode cannot exceed 50 characters";
        public const string TransactionIdRequired = "Transaction ID is required";
        public const string TransactionIdMaxLength = "Transaction ID cannot exceed 20 characters";
        public const string PolicyIdRequired = "Policy ID is required";
    }
}
