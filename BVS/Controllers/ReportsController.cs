using BVS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BVS.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ReportsController(ApplicationDbContext db) { _db = db; }

        public async Task<IActionResult> VideoInventory()
        {
            var videos = await _db.Videos
                .Include(v => v.Rentals)
                .OrderBy(v => v.Title)
                .ToListAsync();
            return View(videos);
        }

        public async Task<IActionResult> CustomerRentals()
        {
            var customers = await _db.Customers
                .Include(c => c.Rentals)
                    .ThenInclude(r => r.Video)
                .Where(c => c.Rentals.Any(r => !r.IsReturned))
                .ToListAsync();
            return View(customers);
        }
    }
}