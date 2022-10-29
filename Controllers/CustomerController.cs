using HoTrungNguyenBTTH2.Data;
using HoTrungNguyenBTTH2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HoTrungNguyenBTTH2.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CustomerController (ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult>Index()
        {
            var model = await _context.Customers.ToListAsync();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Customer std)
        {
            if(ModelState.IsValid)
            {
                _context.Add(std);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(std);
        }
        
        private bool CustomerExists(String id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }
        
        public async Task<IActionResult> Edit(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if(customer == null)
            {
                return NotFound();

            }
            return View(customer);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CustomerID,CustomerName")] Customer std)
        {
            if(id != std.CustomerID)
                return NotFound();
            if(ModelState.IsValid)
            {
                try
                {
                    _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!CustomerExists(std.CustomerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }   
                }
                return RedirectToAction(nameof(Index));
            }
            return View(std);
        }
        
        public async Task<IActionResult> Delete(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var std = await _context.Customers.FirstOrDefaultAsync(m => m.CustomerID == id);
            if (std == null)
                return NotFound();
            return View(std);    
        }
        
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var std = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(std);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}