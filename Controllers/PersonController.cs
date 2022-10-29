using HoTrungNguyenBTTH2.Data;
using HoTrungNguyenBTTH2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HoTrungNguyenBTTH2.Controllers
{
    public class PersonController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PersonController (ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult>Index()
        {
            var model = await _context.Persons.ToListAsync();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Person std)
        {
            if(ModelState.IsValid)
            {
                _context.Add(std);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(std);
        }
        //kiem tra Person co ton tai khong
        private bool PersonExists(String name)
        {
            return _context.Persons.Any(e => e.PersonName == name);
        }
        //kiem tra xem id cua sinh vien co ton tai trong csdl? Co tra ve view "Edit"
        public async Task<IActionResult> Edit(string name)
        {
            if(name == null)
            {
                return NotFound();
            }
            var person = await _context.Persons.FindAsync(name);
            if(person == null)
            {
                return NotFound();

            }
            return View(person);
        }
        //Phuong thuc edit cap nhap thong tin nguoi theo ma ten
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PersonAge,PersonName")] Person std)
        {
            if(id != std.PersonName)
                return NotFound();
            if(ModelState.IsValid)
            {
                try
                {
                    _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!PersonExists(std.PersonName))
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
        //Phuong thuc delete Kiem tra . Co tra ve view Delete
        public async Task<IActionResult> Delete(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var std = await _context.Persons.FirstOrDefaultAsync(m => m.PersonName == id);
            if (std == null)
                return NotFound();
            return View(std);    
        }
        //PHUONG THUC DELETE xoa thong tin sinh vien theo ma id
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var std = await _context.Persons.FindAsync(id);
            _context.Persons.Remove(std);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}