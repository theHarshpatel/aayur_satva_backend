using System;
using System.ComponentModel.DataAnnotations;

namespace AayurSatva.Models
{
    public class History
    {
        [Key]
        public int HistoryId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public string Action { get; set; } = string.Empty; // e.g., "Login", "Booked Appointment", etc.
        
        public DateTime Date { get; set; } = DateTime.Now;
        
        // Navigation properties
        public virtual User? User { get; set; }
    }
}
