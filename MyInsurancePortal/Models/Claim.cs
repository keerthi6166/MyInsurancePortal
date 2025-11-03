using System;

namespace MyInsurancePortal.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }
        public string ClaimNumber { get; set; }
        public DateTime ClaimDate { get; set; }
        public decimal ClaimAmount { get; set; }
        public string Status { get; set; } = "Pending"; // default value
        public string? Description { get; set; }  // optional

        // Foreign Key
        public int PolicyId { get; set; }

        // Navigation Property
        public Policy Policy { get; set; }
    }
}
