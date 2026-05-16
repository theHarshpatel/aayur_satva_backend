using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AayurSatva.Data;
using AayurSatva.Models;

namespace AayurSatva.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicineController : ControllerBase
    {
        private readonly AayurSatvaDbContext _context;

        public MedicineController(AayurSatvaDbContext context)
        {
            _context = context;
        }

        // GET: api/Medicine
        [HttpGet]
        public async Task<IActionResult> GetMedicines([FromQuery] string? coId, [FromQuery] string? yearId)
        {
            var query = _context.Medicines.AsQueryable();
            
            if (!string.IsNullOrEmpty(coId))
            {
                var id = int.Parse(new string(coId.Where(char.IsDigit).ToArray()));
                query = query.Where(m => m.CoId == id);
            }
            if (!string.IsNullOrEmpty(yearId))
            {
                var id = int.Parse(new string(yearId.Where(char.IsDigit).ToArray()));
                query = query.Where(m => m.YearId == id);
            }

            var medicines = await query.ToListAsync();
            return Ok(medicines.Select(m => new { 
                MedId = $"MED{m.MedId:D2}", 
                MedName = m.MedName,
                Price = m.Price,
                Stock = m.Stock
            }));
        }

        // POST: api/Medicine
        [HttpPost]
        public async Task<IActionResult> AddUpdateMedicine([FromBody] AddMedicineRequest request)
        {
            var medId = 0;
            bool isAdd = string.IsNullOrEmpty(request.MedId);
            
            if (!isAdd)
            {
                medId = int.Parse(request.MedId.Substring(3));
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
            
            var medicine = new Medicine 
            { 
                MedId = medId, 
                MedName = request.MedName ?? "",
                Price = request.Price,
                Stock = request.Stock,
                CoId = coId,
                YearId = yearId
            };
            
            if (isAdd) _context.Medicines.Add(medicine);
            else _context.Medicines.Update(medicine);
            
            await _context.SaveChangesAsync();
            return Ok(new { 
                MedId = $"MED{medicine.MedId:D2}", 
                MedName = medicine.MedName,
                Price = medicine.Price,
                Stock = medicine.Stock,
                CoId = $"C{medicine.CoId:D3}",
                YearId = $"Y{medicine.YearId:D3}"
            });
        }

        // POST: api/Medicine/Delete
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteMedicine([FromBody] DeleteMedicineRequest request)
        {
            var idStr = new string(request.Id.Where(char.IsDigit).ToArray());
            if (!int.TryParse(idStr, out int id)) return BadRequest();

            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null) return NotFound(new { message = "Not found" });
            
            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Deleted successfully" });
        }
    }

    public class AddMedicineRequest
    {
        public string? MedId { get; set; }
        public string? MedName { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? CoId { get; set; }
        public string? YearId { get; set; }
    }

    public class DeleteMedicineRequest
    {
        public string Id { get; set; } = string.Empty;
    }
}
