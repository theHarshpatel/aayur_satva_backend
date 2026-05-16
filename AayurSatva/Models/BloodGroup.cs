using System.ComponentModel.DataAnnotations;

namespace AayurSatva.Models
{
    public class BloodGroup
    {
        [Key]
        public int BgId { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string BgName { get; set; } = string.Empty;
    }
}
