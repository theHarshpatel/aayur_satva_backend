using System.ComponentModel.DataAnnotations;

namespace AayurSatva.Models
{
    public class Year
    {
        [Key]
        public int YearId { get; set; }
        
        [Required]
        public string YearName { get; set; } = string.Empty;
    }
}
