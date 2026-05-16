using System.ComponentModel.DataAnnotations;

namespace AayurSatva.Models
{
    public class Medicine
    {
        [Key]
        public int MedId { get; set; }
        
        [Required]
        public string MedName { get; set; } = string.Empty;
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public int Stock { get; set; }
        
        public int CoId { get; set; }
        public int YearId { get; set; }
    }
}
