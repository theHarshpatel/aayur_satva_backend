using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AayurSatva.Data;
using AayurSatva.Models;

namespace AayurSatva.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuManagerController : ControllerBase
    {
        private readonly AayurSatvaDbContext _context;

        public MenuManagerController(AayurSatvaDbContext context)
        {
            _context = context;
        }

        // GET: api/MenuManager
        [HttpGet]
        public async Task<IActionResult> GetMenus()
        {
            var menus = await _context.MenuManagers.ToListAsync();
            return Ok(menus.Select(m => new {
                MenuId = $"MENU{m.Id:D2}",
                m.MenuName,
                m.Title,
                m.MenuIcon,
                m.SubMenuName,
                m.SubMenuIcon,
                m.InternalAccess,
                m.CanAdd,
                m.CanEdit,
                m.CanDelete,
                m.CanView
            }));
        }

        // POST: api/MenuManager
        [HttpPost]
        public async Task<IActionResult> AddUpdateMenu([FromBody] AddMenuRequest request)
        {
            int id = 0;
            bool isAdd = true;

            if (!string.IsNullOrEmpty(request.MenuId))
            {
                var idStr = new string(request.MenuId.Where(char.IsDigit).ToArray());
                if (int.TryParse(idStr, out int parsedId))
                {
                    id = parsedId;
                    isAdd = false;
                }
            }

            var menu = new MenuManager
            {
                Id = id,
                MenuName = request.MenuName ?? "",
                Title = request.Title,
                MenuIcon = request.MenuIcon,
                SubMenuName = request.SubMenuName,
                SubMenuIcon = request.SubMenuIcon,
                InternalAccess = request.InternalAccess,
                CanAdd = request.InternalAccess && request.CanAdd,
                CanEdit = request.InternalAccess && request.CanEdit,
                CanDelete = request.InternalAccess && request.CanDelete,
                CanView = request.InternalAccess && request.CanView
            };

            if (isAdd)
            {
                _context.MenuManagers.Add(menu);
            }
            else
            {
                var existingMenu = await _context.MenuManagers.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                if (existingMenu == null) return NotFound(new { message = "Menu not found" });
                _context.MenuManagers.Update(menu);
            }

            await _context.SaveChangesAsync();

            return Ok(new {
                MenuId = $"MENU{menu.Id:D2}",
                menu.MenuName,
                menu.Title,
                menu.MenuIcon,
                menu.SubMenuName,
                menu.SubMenuIcon,
                menu.InternalAccess,
                menu.CanAdd,
                menu.CanEdit,
                menu.CanDelete,
                menu.CanView
            });
        }

        // POST: api/MenuManager/Delete
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteMenu([FromBody] DeleteMenuRequest request)
        {
            var idStr = new string(request.MenuId.Where(char.IsDigit).ToArray());
            if (!int.TryParse(idStr, out int id)) return BadRequest(new { message = "Invalid ID format" });

            var menu = await _context.MenuManagers.FindAsync(id);
            if (menu == null) return NotFound(new { message = "Not found" });

            _context.MenuManagers.Remove(menu);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Deleted successfully" });
        }
    }

    public class AddMenuRequest
    {
        public string? MenuId { get; set; }
        public string? MenuName { get; set; }
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

    public class DeleteMenuRequest
    {
        public string MenuId { get; set; } = string.Empty;
    }
}
