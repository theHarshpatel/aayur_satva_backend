using System;
using System.ComponentModel.DataAnnotations;

namespace AayurSatva.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Required]
        public string UserName { get; set; } = string.Empty;
        
        public string? FullName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Mobile { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
        
        public string? BloodGroup { get; set; }
        
        public int Role { get; set; } // 0: Admin, 1: Doctor, 2: Patient, 3: Recipiencies
        
        public string? Address { get; set; }
        
        public string? Pincode { get; set; }
        
        public string? State { get; set; }
        
        public bool IsActive { get; set; } = false; // App access
        
        public bool Rights { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}