using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AayurSatva.Data;
using AayurSatva.Models;

namespace AayurSatva.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillingController : ControllerBase
    {
        private readonly AayurSatvaDbContext _context;

        public BillingController(AayurSatvaDbContext context)
        {
            _context = context;
        }

        // GET: api/Billing/GetAllBill
        [HttpGet("GetAllBill")]
        public async Task<IActionResult> GetAllBills([FromQuery] string? coId, [FromQuery] string? yearId)
        {
            var query = _context.Billings.AsQueryable();
            
            if (!string.IsNullOrEmpty(coId))
            {
                var id = int.Parse(new string(coId.Where(char.IsDigit).ToArray()));
                query = query.Where(b => b.CoId == id);
            }
            if (!string.IsNullOrEmpty(yearId))
            {
                var id = int.Parse(new string(yearId.Where(char.IsDigit).ToArray()));
                query = query.Where(b => b.YearId == id);
            }

            var bills = await query.ToListAsync();
            return Ok(bills.Select(b => new {
                BillingId = $"BILL{b.BillingId:D2}",
                BillNo = b.BillNo,
                UserId = $"USER{b.UserId:D2}",
                TotalAmount = b.TotalAmount,
                BillDate = b.BillDate
            }));
        }

        // GET: api/Billing/User/{userId}
        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetUserBills(string userId, [FromQuery] string? coId, [FromQuery] string? yearId)
        {
            var idStr = new string(userId.Where(char.IsDigit).ToArray());
            if (!int.TryParse(idStr, out int id)) return BadRequest(new { message = "Invalid User ID format" });

            var query = _context.Billings.Where(b => b.UserId == id).AsQueryable();
            
            if (!string.IsNullOrEmpty(coId))
            {
                var cid = int.Parse(new string(coId.Where(char.IsDigit).ToArray()));
                query = query.Where(b => b.CoId == cid);
            }
            if (!string.IsNullOrEmpty(yearId))
            {
                var yid = int.Parse(new string(yearId.Where(char.IsDigit).ToArray()));
                query = query.Where(b => b.YearId == yid);
            }

            var bills = await query.ToListAsync();
            return Ok(bills.Select(b => new {
                BillingId = $"BILL{b.BillingId:D2}",
                BillNo = b.BillNo,
                UserId = $"USER{b.UserId:D2}",
                TotalAmount = b.TotalAmount,
                BillDate = b.BillDate
            }));
        }

        // POST: api/Billing/AddBill
        [HttpPost("AddBill")]
        public async Task<IActionResult> AddUpdateBill([FromBody] AddBillingRequest request)
        {
            var billingId = 0;
            bool isAdd = string.IsNullOrEmpty(request.BillingId);

            if (!isAdd)
            {
                var idStr = new string(request.BillingId!.Where(char.IsDigit).ToArray());
                if (int.TryParse(idStr, out int id))
                {
                    billingId = id;
                }
            }

            var userId = 0;
            if (!string.IsNullOrEmpty(request.UserId)) {
                var uIdStr = new string(request.UserId.Where(char.IsDigit).ToArray());
                int.TryParse(uIdStr, out userId);
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

            var bill = new Billing
            {
                BillingId = billingId,
                BillNo = request.BillNo ?? "",
                UserId = userId,
                TotalAmount = request.TotalAmount,
                BillDate = request.BillDate != default ? request.BillDate : DateTime.Now,
                CoId = coId,
                YearId = yearId
            };

            if (isAdd) _context.Billings.Add(bill);
            else _context.Billings.Update(bill);

            await _context.SaveChangesAsync();
            
            return Ok(new {
                BillingId = $"BILL{bill.BillingId:D2}",
                BillNo = bill.BillNo,
                UserId = $"USER{bill.UserId:D2}",
                TotalAmount = bill.TotalAmount,
                BillDate = bill.BillDate,
                CoId = $"C{bill.CoId:D3}",
                YearId = $"Y{bill.YearId:D3}"
            });
        }

        // POST: api/Billing/DeleteBill
        [HttpPost("DeleteBill")]
        public async Task<IActionResult> DeleteBill([FromBody] DeleteBillingRequest request)
        {
            var idStr = new string(request.Id.Where(char.IsDigit).ToArray());
            if (!int.TryParse(idStr, out int id)) return BadRequest(new { message = "Invalid ID format" });

            var bill = await _context.Billings.FindAsync(id);
            if (bill == null) return NotFound(new { message = "Not found" });
            _context.Billings.Remove(bill);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Deleted successfully" });
        }
    }

    public class AddBillingRequest
    {
        public string? BillingId { get; set; }
        public string? BillNo { get; set; }
        public string? UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime BillDate { get; set; }
        public string? CoId { get; set; }
        public string? YearId { get; set; }
    }

    public class DeleteBillingRequest
    {
        public string Id { get; set; } = string.Empty;
    }
}
