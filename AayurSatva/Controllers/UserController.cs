using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AayurSatva.Data;
using AayurSatva.Models;

namespace AayurSatva.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AayurSatvaDbContext _context;

        public UserController(AayurSatvaDbContext context)
        {
            _context = context;
        }

        // GET: api/User
        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] string? userId, [FromQuery] string? ruserId)
        {
            if (!string.IsNullOrEmpty(userId) || !string.IsNullOrEmpty(ruserId))
            {
                int id = 0;
                string searchId = userId ?? ruserId!;
                var idStr = new string(searchId.Where(char.IsDigit).ToArray());
                if (int.TryParse(idStr, out int parsedId)) id = parsedId;

                if (id == 0) return BadRequest(new { message = "Invalid ID format" });

                var user = await _context.Users.FindAsync(id);
                if (user == null) return NotFound();

                // Strict check for formatted ID
                string formattedUserId = $"USER{user.UserId:D2}";
                string formattedRuserId = user.Role == 0 ? $"A{user.UserId:D3}" :
                                          user.Role == 1 ? $"DOC{user.UserId:D2}" :
                                          user.Role == 2 ? $"PET{user.UserId:D2}" :
                                          user.Role == 3 ? $"REC{user.UserId:D2}" : $"USER{user.UserId:D2}";

                if (searchId != formattedUserId && searchId != formattedRuserId)
                {
                    return NotFound(new { message = "User not found with this specific ID format." });
                }

                return Ok(new {
                    UserId = $"USER{user.UserId:D2}",
                    RuserId = user.Role == 0 ? $"A{user.UserId:D3}" :
                              user.Role == 1 ? $"DOC{user.UserId:D2}" :
                              user.Role == 2 ? $"PET{user.UserId:D2}" :
                              user.Role == 3 ? $"REC{user.UserId:D2}" : $"USER{user.UserId:D2}",
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Mobile = user.Mobile,
                    Password = user.Password,
                    BloodGroup = user.BloodGroup,
                    Role = user.Role,
                    Address = user.Address,
                    Pincode = user.Pincode,
                    State = user.State,
                    IsActive = user.IsActive,
                    Rights = user.Rights,
                    CreatedAt = user.CreatedAt
                });
            }

            var users = await _context.Users.ToListAsync();
            return Ok(users.Select(u => new {
                UserId = $"USER{u.UserId:D2}",
                RuserId = u.Role == 0 ? $"A{u.UserId:D3}" :
                          u.Role == 1 ? $"DOC{u.UserId:D2}" :
                          u.Role == 2 ? $"PET{u.UserId:D2}" :
                          u.Role == 3 ? $"REC{u.UserId:D2}" : $"USER{u.UserId:D2}",
                UserName = u.UserName,
                FullName = u.FullName,
                Email = u.Email,
                Mobile = u.Mobile,
                Password = u.Password,
                BloodGroup = u.BloodGroup,
                Role = u.Role,
                Address = u.Address,
                Pincode = u.Pincode,
                State = u.State,
                IsActive = u.IsActive,
                Rights = u.Rights,
                CreatedAt = u.CreatedAt
            }));
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> AddUpdateUser([FromBody] AddUserRequest request)
        {
            var userId = 0;
            bool isAdd = string.IsNullOrEmpty(request.UserId);

            if (!isAdd)
            {
                var idStr = new string(request.UserId!.Where(char.IsDigit).ToArray());
                if (int.TryParse(idStr, out int id))
                {
                    userId = id;
                }
            }

            // Check for duplicates
            var existingUsers = await _context.Users
                .Where(u => u.UserId != userId && 
                            (u.UserName == request.UserName || 
                             (!string.IsNullOrEmpty(request.FullName) && u.FullName == request.FullName) || 
                             u.Email == request.Email || 
                             u.Mobile == request.Mobile))
                .ToListAsync();

            if (existingUsers.Any())
            {
                bool usernameMatch = existingUsers.Any(u => u.UserName == request.UserName);
                bool fullnameMatch = existingUsers.Any(u => !string.IsNullOrEmpty(request.FullName) && u.FullName == request.FullName);
                bool emailMatch = existingUsers.Any(u => u.Email == request.Email);
                bool mobileMatch = existingUsers.Any(u => u.Mobile == request.Mobile);

                if (emailMatch && mobileMatch)
                {
                    return BadRequest(new { message = "email & mobile is already exists." });
                }

                var errors = new List<string>();
                if (usernameMatch) errors.Add("username already exists.");
                if (fullnameMatch) errors.Add("fullname already exists.");
                if (emailMatch) errors.Add("email already exists.");
                if (mobileMatch) errors.Add("mobileno already exists.");

                return BadRequest(new { message = string.Join(" ", errors) });
            }

            var user = new User
            {
                UserId = userId,
                UserName = request.UserName ?? "",
                FullName = request.FullName,
                Email = request.Email ?? "",
                Mobile = request.Mobile ?? "",
                Password = request.Password ?? "",
                BloodGroup = request.BloodGroup,
                Role = request.Role,
                Address = request.Address,
                Pincode = request.Pincode,
                State = request.State,
                IsActive = request.IsActive,
                Rights = request.Rights,
                CreatedAt = DateTime.Now
            };

            if (isAdd) _context.Users.Add(user);
            else {
                // To avoid overwriting CreatedAt if not in request, but since we recreate the user, it will be DateTime.Now unless we preserve it. 
                // Let's preserve it by fetching the existing user first for updates.
                var existingUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
                if (existingUser != null)
                {
                    user.CreatedAt = existingUser.CreatedAt;
                    if (string.IsNullOrEmpty(request.Password)) user.Password = existingUser.Password;
                }
                _context.Users.Update(user);
            }

            await _context.SaveChangesAsync();
            
            return Ok(new {
                UserId = $"USER{user.UserId:D2}",
                RuserId = user.Role == 0 ? $"A{user.UserId:D3}" :
                          user.Role == 1 ? $"DOC{user.UserId:D2}" :
                          user.Role == 2 ? $"PET{user.UserId:D2}" :
                          user.Role == 3 ? $"REC{user.UserId:D2}" : $"USER{user.UserId:D2}",
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Mobile = user.Mobile,
                Password = user.Password,
                BloodGroup = user.BloodGroup,
                Role = user.Role,
                Address = user.Address,
                Pincode = user.Pincode,
                State = user.State,
                IsActive = user.IsActive,
                Rights = user.Rights,
                CreatedAt = user.CreatedAt
            });
        }

        // POST: api/User/Delete
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequest request)
        {
            var idStr = new string(request.Id.Where(char.IsDigit).ToArray());
            if (!int.TryParse(idStr, out int id)) return BadRequest(new { message = "Invalid ID format" });

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound(new { message = "Not found" });
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Deleted successfully" });
        }

        // POST: api/User/Rights
        [HttpPost("Rights")]
        public async Task<IActionResult> UpdateUserRights([FromBody] UserRightsRequest request)
        {
            var idStr = request.UserId == null ? "" : new string(request.UserId.Where(char.IsDigit).ToArray());
            if (!int.TryParse(idStr, out int userId)) return BadRequest();

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound(new { message = "Not found" });

            // Remove existing access
            var existingAccess = _context.UserMenuAccesses.Where(a => a.UserId == userId);
            _context.UserMenuAccesses.RemoveRange(existingAccess);

            // Add new access
            foreach (var menuIdStr in request.MenuIds)
            {
                var mIdStr = new string(menuIdStr.Where(char.IsDigit).ToArray());
                if (int.TryParse(mIdStr, out int menuId))
                {
                    _context.UserMenuAccesses.Add(new UserMenuAccess { UserId = userId, MenuId = menuId });
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new {
                UserId = $"USER{user.UserId:D2}",
                RuserId = user.Role == 0 ? $"A{user.UserId:D3}" :
                          user.Role == 1 ? $"DOC{user.UserId:D2}" :
                          user.Role == 2 ? $"PET{user.UserId:D2}" :
                          user.Role == 3 ? $"REC{user.UserId:D2}" : $"USER{user.UserId:D2}",
                MenuIds = request.MenuIds
            });
        }

        // POST: api/User/AppAccess
        [HttpPost("AppAccess")]
        public async Task<IActionResult> UpdateAppAccess([FromBody] AppAccessRequest request)
        {
            var idStr = request.UserId == null ? "" : new string(request.UserId.Where(char.IsDigit).ToArray());
            if (!int.TryParse(idStr, out int userId)) return BadRequest();

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound(new { message = "Not found" });
            user.IsActive = request.IsActive;
            await _context.SaveChangesAsync();
            return Ok(new {
                UserId = $"USER{user.UserId:D2}",
                RuserId = user.Role == 0 ? $"A{user.UserId:D3}" :
                          user.Role == 1 ? $"DOC{user.UserId:D2}" :
                          user.Role == 2 ? $"PET{user.UserId:D2}" :
                          user.Role == 3 ? $"REC{user.UserId:D2}" : $"USER{user.UserId:D2}",
                IsActive = user.IsActive
            });
        }
    }

    public class AddUserRequest
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Password { get; set; }
        public string? BloodGroup { get; set; }
        public int Role { get; set; }
        public string? Address { get; set; }
        public string? Pincode { get; set; }
        public string? State { get; set; }
        public bool IsActive { get; set; }
        public bool Rights { get; set; }
    }

    public class DeleteUserRequest
    {
        public string Id { get; set; } = string.Empty;
    }

    public class UserRightsRequest
    {
        public string? UserId { get; set; }
        public List<string> MenuIds { get; set; } = new List<string>();
    }

    public class AppAccessRequest
    {
        public string? UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
