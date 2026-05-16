using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AayurSatva.Data;
using AayurSatva.Models;

namespace AayurSatva.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AayurSatvaDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AayurSatvaDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => (u.UserName == request.Username || u.Email == request.Username || u.Mobile == request.Username) && u.Password == request.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid credentials!" });

            if (!user.IsActive)
                return Unauthorized(new { message = "Account is not active!" });

            var companies = await _context.Companies.ToListAsync();
            
            return Ok(new { 
                message = "Login successful!", 
                userId = $"USER{user.UserId:D2}", 
                role = user.Role,
                userName = user.UserName,
                fullName = user.FullName ?? user.UserName,
                company = companies.Select(c => new {
                    coId = $"C{c.CoId:D3}",
                    coName = c.CoName
                })
            });
        }

        // POST: api/Auth/token
        [HttpPost("token")]
        public async Task<IActionResult> GenerateToken([FromBody] TokenRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.Username);

            if (user == null)
                return NotFound(new { message = "User not found!" });

            var coId = 0;
            if (!string.IsNullOrEmpty(request.CoId))
            {
                coId = int.Parse(new string(request.CoId.Where(char.IsDigit).ToArray()));
            }

            var yearId = 0;
            if (!string.IsNullOrEmpty(request.YearId))
            {
                yearId = int.Parse(new string(request.YearId.Where(char.IsDigit).ToArray()));
            }

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "DefaultSecretKey");
            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName),
                    new System.Security.Claims.Claim("userId", user.UserId.ToString()),
                    new System.Security.Claims.Claim("cid", coId.ToString()),
                    new System.Security.Claims.Claim("yearId", yearId.ToString()),
                    new System.Security.Claims.Claim("platform", request.Platform ?? "W")
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:ExpiryMinutes"] ?? "60")),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new {
                databaseName = "AayurSatva",
                cocode = coId,
                cid = coId,
                serverId = 1,
                userID = $"USER{user.UserId:D2}",
                fullName = user.FullName ?? user.UserName,
                userType = user.Role,
                mobileNo = user.Mobile,
                email = user.Email,
                profileImg = (string?)null,
                ledgerStart = (string?)null,
                ledgerEnd = (string?)null,
                secodEs = "",
                pcodEs = "",
                token = tokenString
            });
        }

        // POST: api/Auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingUsers = await _context.Users
                .Where(u => u.UserName == request.UserName || 
                            (!string.IsNullOrEmpty(request.FullName) && u.FullName == request.FullName) || 
                            u.Email == request.Email || 
                            u.Mobile == request.Mobile)
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
                UserName = request.UserName,
                FullName = request.FullName,
                Email = request.Email,
                Mobile = request.Mobile,
                Password = request.Password,
                Role = 2, // Default to Patient
                IsActive = false, // Default false
                Rights = false // Default false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registered successfully!", userId = user.UserId });
        }

        // POST: api/Auth/reset-password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.UserName);

            if (user == null)
                return NotFound(new { message = "User not found!" });

            user.Password = request.Password;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Password reset successful!" });
        }

        // GET: api/Auth/company
        [HttpGet("company")]
        public async Task<IActionResult> GetCompany([FromQuery] int coid, [FromQuery] int yearid)
        {
            var company = await _context.Companies.FindAsync(coid);
            var year = await _context.Years.FindAsync(yearid);

            if (company == null || year == null)
                return NotFound(new { message = "Company or Year not found!" });

            return Ok(new { company = company.CoName, year = year.YearName });
        }

    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty; // Mobile, Email or Username
        public string Password { get; set; } = string.Empty;
    }

    public class TokenRequest
    {
        public string Username { get; set; } = string.Empty;
        public string CoId { get; set; } = string.Empty;
        public string YearId { get; set; } = string.Empty;
        public string? Platform { get; set; }
    }

    public class RegisterRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ResetPasswordRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

}
