using BVS.Data;
using BVS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BVS.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CustomersController(ApplicationDbContext db) { _db = db; }

        public async Task<IActionResult> Index()
            => View(await _db.Customers.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,Phone,Email,Address")] Customer c)
        {
            if (ModelState.IsValid)
            {
                _db.Customers.Add(c);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return View(c);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var c = await _db.Customers.FindAsync(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,FullName,Phone,Email,Address")] Customer c)
        {
            if (id != c.CustomerID) return NotFound();
            if (ModelState.IsValid)
            {
                _db.Customers.Update(c);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(c);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Customers.FindAsync(id);
            if (c == null) return NotFound();
            _db.Customers.Remove(c);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}