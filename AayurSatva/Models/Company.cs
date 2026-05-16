using System.ComponentModel.DataAnnotations;

namespace AayurSatva.Models
{
    public class Company
    {
        [Key]
        public int CoId { get; set; }
        
        [Required]
        public string CoName { get; set; } = string.Empty;
    }
}