using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AayurSatva.Data;
using AayurSatva.Models;

namespace AayurSatva.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AayurSatvaDbContext _context;

        public AppointmentController(AayurSatvaDbContext context)
        {
            _context = context;
        }

        // GET: api/Appointment/GetAllAppointment
        [HttpGet("GetAllAppointment")]
        public async Task<IActionResult> GetAllAppointments([FromQuery] string? coId, [FromQuery] string? yearId)
        {
            var query = _context.Appointments.AsQueryable();
            
            if (!string.IsNullOrEmpty(coId))
            {
                var id = int.Parse(new string(coId.Where(char.IsDigit).ToArray()));
                query = query.Where(a => a.CoId == id);
            }
            if (!string.IsNullOrEmpty(yearId))
            {
                var id = int.Parse(new string(yearId.Where(char.IsDigit).ToArray()));
                query = query.Where(a => a.YearId == id);
            }

            var appointments = await query.ToListAsync();
            return Ok(appointments.Select(a => new {
                AppointmentId = $"APT{a.AppointmentId:D2}",
                UserId = $"USER{a.UserId:D2}",
                DoctorId = $"USER{a.DoctorId:D2}",
                AppointmentDate = a.AppointmentDate,
                Token = a.Token,
                Status = a.Status,
                CreatedAt = a.CreatedAt
            }));
        }

        // POST: api/Appointment/AddAppointment
        [HttpPost("AddAppointment")]
        public async Task<IActionResult> AddUpdateAppointment([FromBody] AddAppointmentRequest request)
        {
            var appointmentId = 0;
            bool isAdd = string.IsNullOrEmpty(request.AppointmentId);

            if (!isAdd)
            {
                var idStr = new string(request.AppointmentId!.Where(char.IsDigit).ToArray());
                if (int.TryParse(idStr, out int id))
                {
                    appointmentId = id;
                }
            }

            var userId = 0;
            if (!string.IsNullOrEmpty(request.UserId)) {
                var uIdStr = new string(request.UserId.Where(char.IsDigit).ToArray());
                int.TryParse(uIdStr, out userId);
            }

            var doctorId = 0;
            if (!string.IsNullOrEmpty(request.DoctorId)) {
                var dIdStr = new string(request.DoctorId.Where(char.IsDigit).ToArray());
                int.TryParse(dIdStr, out doctorId);
            }

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

            var appointment = new Appointment
            {
                AppointmentId = appointmentId,
                UserId = userId,
                DoctorId = doctorId,
                AppointmentDate = request.AppointmentDate,
                Token = request.Token ?? "",
                Status = request.Status ?? "Pending",
                CoId = coId,
                YearId = yearId
            };

            if (isAdd) _context.Appointments.Add(appointment);
            else _context.Appointments.Update(appointment);

            await _context.SaveChangesAsync();
            
            return Ok(new {
                AppointmentId = $"APT{appointment.AppointmentId:D2}",
                UserId = $"USER{appointment.UserId:D2}",
                DoctorId = $"USER{appointment.DoctorId:D2}",
                AppointmentDate = appointment.AppointmentDate,
                Token = appointment.Token,
                Status = appointment.Status,
                CreatedAt = appointment.CreatedAt,
                CoId = $"C{appointment.CoId:D3}",
                YearId = $"Y{appointment.YearId:D3}"
            });
        }

        // POST: api/Appointment/DeleteAppointment
        [HttpPost("DeleteAppointment")]
        public async Task<IActionResult> DeleteAppointment([FromBody] DeleteAppointmentRequest request)
        {
            var idStr = new string(request.Id.Where(char.IsDigit).ToArray());
            if (!int.TryParse(idStr, out int id)) return BadRequest(new { message = "Invalid ID format" });

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound(new { message = "Not found" });
            
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Deleted successfully" });
        }
    }

    public class AddAppointmentRequest
    {
        public string? AppointmentId { get; set; }
        public string? UserId { get; set; }
        public string? DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? Token { get; set; }
        public string? Status { get; set; }
        public string? CoId { get; set; }
        public string? YearId { get; set; }
    }

    public class DeleteAppointmentRequest
    {
        public string Id { get; set; } = string.Empty;
    }
}
