using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AayurSatva.Data;
using AayurSatva.Models;

namespace AayurSatva.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MasterController : ControllerBase
    {
        private readonly AayurSatvaDbContext _context;

        public MasterController(AayurSatvaDbContext context)
        {
            _context = context;
        }

        // GET: api/Master/Admins
        [HttpGet("Admins")]
        public async Task<IActionResult> GetAdmins()
        {
            var admins = await _context.Users.Where(u => u.Role == 0).ToListAsync();
            return Ok(admins.Select(u => new {
                UserId = $"USER{u.UserId:D2}",
                RuserId = $"A{u.UserId:D3}",
                UserName = u.UserName
            }));
        }

        // GET: api/Master/Patients
        [HttpGet("Patients")]
        public async Task<IActionResult> GetPatients()
        {
            var patients = await _context.Users.Where(u => u.Role == 2).ToListAsync();
            return Ok(patients.Select(u => new {
                UserId = $"USER{u.UserId:D2}",
                RuserId = $"PET{u.UserId:D2}",
                UserName = u.UserName
            }));
        }

        // GET: api/Master/Doctors
        [HttpGet("Doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _context.Users.Where(u => u.Role == 1).ToListAsync();
            return Ok(doctors.Select(u => new {
                UserId = $"USER{u.UserId:D2}",
                RuserId = $"DOC{u.UserId:D2}",
                UserName = u.UserName
            }));
        }

        // GET: api/Master/Recipiencies
        [HttpGet("Recipiencies")]
        public async Task<IActionResult> GetRecipiencies()
        {
            var recipiencies = await _context.Users.Where(u => u.Role == 3).ToListAsync();
            return Ok(recipiencies.Select(u => new {
                UserId = $"USER{u.UserId:D2}",
                RuserId = $"REC{u.UserId:D2}",
                UserName = u.UserName
            }));
        }

        // GET: api/Master/Companies
        [HttpGet("Companies")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _context.Companies.ToListAsync();
            return Ok(companies.Select(c => new { CoId = $"C{c.CoId:D3}", CoName = c.CoName }));
        }

        // GET: api/Master/Years
        [HttpGet("Years")]
        public async Task<IActionResult> GetYears()
        {
            var years = await _context.Years.ToListAsync();
            return Ok(years.Select(y => new { YearId = $"Y{y.YearId:D3}", YearName = y.YearName }));
        }

        // GET: api/Master/States
        [HttpGet("States")]
        public async Task<IActionResult> GetStates()
        {
            var states = await _context.States.ToListAsync();
            return Ok(states.Select(s => new { StateId = $"STATE{s.StateId:D2}", StateName = s.StateName }));
        }

        // GET: api/Master/Cities
        [HttpGet("Cities")]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _context.Cities.ToListAsync();
            return Ok(cities.Select(c => new { CityId = $"CITY{c.CityId:D2}", CityName = c.CityName }));
        }

        // GET: api/Master/BloodGroups
        [HttpGet("BloodGroups")]
        public async Task<IActionResult> GetBloodGroups()
        {
            var bloodGroups = await _context.BloodGroups.ToListAsync();
            return Ok(bloodGroups.Select(bg => new { BgId = $"BG{bg.BgId:D3}", BgName = bg.BgName }));
        }



        // POST: api/AddMaster
        [HttpPost("/api/AddMaster")]
        public async Task<IActionResult> AddMaster([FromBody] AddMasterRequest request)
        {
            if (!string.IsNullOrEmpty(request.CoName) || request.CoId != null)
            {
                var coId = 0;
                if (!string.IsNullOrEmpty(request.CoId))
                {
                    coId = int.Parse(request.CoId.Substring(1));
                }
                
                var company = new Company { CoId = coId, CoName = request.CoName ?? "" };
                if (coId == 0) _context.Companies.Add(company);
                else _context.Companies.Update(company);
                
                await _context.SaveChangesAsync();
                return Ok(new { CoId = $"C{company.CoId:D3}", CoName = company.CoName });
            }
            else if (!string.IsNullOrEmpty(request.CityName) || request.CityId != null)
            {
                var cityId = 0;
                if (!string.IsNullOrEmpty(request.CityId))
                {
                    cityId = int.Parse(request.CityId.Substring(4));
                }
                
                var city = new City { CityId = cityId, CityName = request.CityName ?? "" };
                if (cityId == 0) _context.Cities.Add(city);
                else _context.Cities.Update(city);
                
                await _context.SaveChangesAsync();
                return Ok(new { CityId = $"CITY{city.CityId:D2}", CityName = city.CityName });
            }
            else if (!string.IsNullOrEmpty(request.StateName) || request.StateId != null)
            {
                var stateId = 0;
                if (!string.IsNullOrEmpty(request.StateId))
                {
                    stateId = int.Parse(request.StateId.Substring(5));
                }
                
                var state = new State { StateId = stateId, StateName = request.StateName ?? "" };
                if (stateId == 0) _context.States.Add(state);
                else _context.States.Update(state);
                
                await _context.SaveChangesAsync();
                return Ok(new { StateId = $"STATE{state.StateId:D2}", StateName = state.StateName });
            }
            else if (!string.IsNullOrEmpty(request.YearName) || request.YearId != null)
            {
                var yearId = 0;
                if (!string.IsNullOrEmpty(request.YearId))
                {
                    yearId = int.Parse(request.YearId.Substring(1));
                }
                
                var year = new Year { YearId = yearId, YearName = request.YearName ?? "" };
                if (yearId == 0) _context.Years.Add(year);
                else _context.Years.Update(year);
                
                await _context.SaveChangesAsync();
                return Ok(new { YearId = $"Y{year.YearId:D3}", YearName = year.YearName });
            }
            else if (!string.IsNullOrEmpty(request.BgName) || request.BgId != null)
            {
                var bgId = 0;
                if (!string.IsNullOrEmpty(request.BgId))
                {
                    bgId = int.Parse(request.BgId.Substring(2));
                }
                
                var bloodGroup = new BloodGroup { BgId = bgId, BgName = request.BgName ?? "" };
                if (bgId == 0) _context.BloodGroups.Add(bloodGroup);
                else _context.BloodGroups.Update(bloodGroup);
                
                await _context.SaveChangesAsync();
                return Ok(new { BgId = $"BG{bloodGroup.BgId:D3}", BgName = bloodGroup.BgName });
            }
            
            return BadRequest(new { message = "Invalid request" });
        }

        // POST: api/DeleteMaster
        [HttpPost("/api/DeleteMaster")]
        public async Task<IActionResult> DeleteMaster([FromBody] DeleteMasterRequest request)
        {
            if (request.Id.StartsWith("C"))
            {
                var id = int.Parse(request.Id.Substring(1));
                var company = await _context.Companies.FindAsync(id);
                if (company == null) return NotFound();
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Deleted successfully" });
            }
            else if (request.Id.StartsWith("Y"))
            {
                var id = int.Parse(request.Id.Substring(1));
                var year = await _context.Years.FindAsync(id);
                if (year == null) return NotFound();
                _context.Years.Remove(year);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Deleted successfully" });
            }
            else if (request.Id.StartsWith("CITY"))
            {
                var id = int.Parse(request.Id.Substring(4));
                var city = await _context.Cities.FindAsync(id);
                if (city == null) return NotFound();
                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Deleted successfully" });
            }
            else if (request.Id.StartsWith("STATE"))
            {
                var id = int.Parse(request.Id.Substring(5));
                var state = await _context.States.FindAsync(id);
                if (state == null) return NotFound();
                _context.States.Remove(state);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Deleted successfully" });
            }
            else if (request.Id.StartsWith("BG"))
            {
                var id = int.Parse(request.Id.Substring(2));
                var bloodGroup = await _context.BloodGroups.FindAsync(id);
                if (bloodGroup == null) return NotFound();
                _context.BloodGroups.Remove(bloodGroup);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Deleted successfully" });
            }
            
            return BadRequest(new { message = "Invalid ID format" });
        }


    }

    public class AddMasterRequest
    {
        public string? CoId { get; set; }
        public string? CoName { get; set; }
        public string? CityId { get; set; }
        public string? CityName { get; set; }
        public string? StateId { get; set; }
        public string? StateName { get; set; }
        public string? YearId { get; set; }
        public string? YearName { get; set; }
        public string? BgId { get; set; }
        public string? BgName { get; set; }
    }

    public class DeleteMasterRequest
    {
        public string Id { get; set; } = string.Empty;
    }

    public class DeleteRequest
    {
        public int Id { get; set; }
    }
}
