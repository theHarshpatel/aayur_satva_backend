using System;
using System.ComponentModel.DataAnnotations;

namespace AayurSatva.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        
        [Required]
        public int UserId { get; set; } // Patient
        
        [Required]
        public int DoctorId { get; set; } // Doctor (User with role 1)
        
        [Required]
        public DateTime AppointmentDate { get; set; }
        
        public string Token { get; set; } = string.Empty; // Token for booking
        
        public string Status { get; set; } = "Pending"; // Pending, Approved, Cancelled
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public int CoId { get; set; }
        public int YearId { get; set; }
        
        // Navigation properties
        public virtual User? User { get; set; }
    }
}