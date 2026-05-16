using System.ComponentModel.DataAnnotations;

namespace AayurSatva.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }
        
        [Required]
        public string CityName { get; set; } = string.Empty;
    }
}
