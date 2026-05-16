using System.ComponentModel.DataAnnotations;

namespace AayurSatva.Models
{
    public class UserMenuAccess
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int MenuId { get; set; }
    }
}
