using System.ComponentModel.DataAnnotations;

namespace AayurSatva.Models
{
    public class MenuManager
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string MenuName { get; set; } = string.Empty;
        
        public string? Title { get; set; }
        
        public string? MenuIcon { get; set; }
        
        public string? SubMenuName { get; set; }
        
        public string? SubMenuIcon { get; set; }
        
        public bool InternalAccess { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanView { get; set; }
    }
}
