using BVS.Data;
using BVS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BVS.Controllers
{
    public class RentalsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public RentalsController(ApplicationDbContext db) { _db = db; }

        public async Task<IActionResult> Index()
        {
            var rentals = await _db.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Video)
                .OrderByDescending(r => r.RentDate)
                .ToListAsync();
            return View(rentals);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Customers = new SelectList(
                await _db.Customers.ToListAsync(), "CustomerID", "FullName");

            var availableVideos = await _db.Videos
                .Where(v => _db.Rentals
                    .Count(r => r.VideoID == v.VideoID && !r.IsReturned) < v.TotalCopies)
                .ToListAsync();

            ViewBag.Videos = new SelectList(availableVideos, "VideoID", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int customerID, int videoID, DateTime rentDate)
        {
            var video = await _db.Videos.FindAsync(videoID);
            if (video == null) return NotFound();

            var rental = new Rental
            {
                CustomerID = customerID,
                VideoID = videoID,
                RentDate = rentDate,
                DueDate = rentDate.AddDays(video.MaxDays),
                IsReturned = false
            };

            _db.Rentals.Add(rental);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Return(int id)
        {
            var rental = await _db.Rentals.FindAsync(id);
            if (rental == null) return NotFound();
            rental.IsReturned = true;
            rental.ReturnDate = DateTime.Today;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}