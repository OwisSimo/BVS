using BVS.Data;
using BVS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BVS.Controllers
{
    public class VideosController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VideosController(ApplicationDbContext db) { _db = db; }

        public async Task<IActionResult> Index()
            => View(await _db.Videos.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Video v)
        {
            if (!ModelState.IsValid) return View(v);
            _db.Videos.Add(v);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var v = await _db.Videos.FindAsync(id);
            if (v == null) return NotFound();
            return View(v);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Video v)
        {
            if (!ModelState.IsValid) return View(v);
            _db.Videos.Update(v);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var hasRentals = await _db.Rentals
                .AnyAsync(r => r.VideoID == id && !r.IsReturned);
            if (hasRentals)
            {
                TempData["Error"] = "Cannot delete a video with active rentals.";
                return RedirectToAction(nameof(Index));
            }
            var v = await _db.Videos.FindAsync(id);
            if (v == null) return NotFound();
            _db.Videos.Remove(v);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}