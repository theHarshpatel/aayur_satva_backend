using System;
using System.ComponentModel.DataAnnotations;

namespace AayurSatva.Models
{
    public class MedicalRecord
    {
        [Key]
        public int RecordId { get; set; }
        
        [Required]
        public int UserId { get; set; } // Patient
        
        [Required]
        public int DoctorId { get; set; } // Doctor
        
        [Required]
        public string Details { get; set; } = string.Empty;
        
        public DateTime Date { get; set; } = DateTime.Now;
        
        // Navigation properties
        public virtual User? User { get; set; }
    }
}