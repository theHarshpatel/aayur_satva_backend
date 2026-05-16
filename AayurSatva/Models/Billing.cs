using System;
using System.ComponentModel.DataAnnotations;

namespace AayurSatva.Models
{
    public class Billing
    {
        [Key]
        public int BillingId { get; set; }
        
        [Required]
        public string BillNo { get; set; } = string.Empty;
        
        [Required]
        public int UserId { get; set; } // Patient
        
        [Required]
        public decimal TotalAmount { get; set; }
        
        public DateTime BillDate { get; set; } = DateTime.Now;
        
        public int CoId { get; set; }
        public int YearId { get; set; }
        
        // Navigation properties
        public virtual User? User { get; set; }
    }
}