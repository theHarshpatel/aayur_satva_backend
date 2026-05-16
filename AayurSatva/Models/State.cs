using System.ComponentModel.DataAnnotations;

namespace AayurSatva.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }
        
        [Required]
        public string StateName { get; set; } = string.Empty;
    }
}
