using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyInsurancePortal.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentMode { get; set; }
        public string TransactionId { get; set; }

        // Foreign Key
        public int PolicyId { get; set; }

        // Navigation Property
        public Policy Policy { get; set; }
    }
}
