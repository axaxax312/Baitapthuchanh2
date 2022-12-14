using HoTrungNguyenBTTH2.Data;
using HoTrungNguyenBTTH2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HoTrungNguyenBTTH2.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentController (ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult>Index()
        {
            var model = await _context.Students.ToListAsync();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Student std)
        {
            if(ModelState.IsValid)
            {
                _context.Add(std);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(std);
        }
        //kiem tra sinh vien co ton tai khong
        private bool StudentExists(String id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }
        //kiem tra xem id cua sinh vien co ton tai trong csdl? Co tra ve view "Edit"
        public async Task<IActionResult> Edit(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var student = await _context.Students.FindAsync(id);
            if(student == null)
            {
                return NotFound();

            }
            return View(student);
        }
        //Phuong thuc edit cap nhap thong tin sinh vien theo ma sinhvien
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StudentID,StudentName")] Student std)
        {
            if(id != std.StudentID)
                return NotFound();
            if(ModelState.IsValid)
            {
                try
                {
                    _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!StudentExists(std.StudentID))
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
        //Phuong thuc delete Kiem tra id. Co tra ve view Delete
        public async Task<IActionResult> Delete(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var std = await _context.Students.FirstOrDefaultAsync(m => m.StudentID == id);
            if (std == null)
                return NotFound();
            return View(std);    
        }
        //PHUONG THUC DELETE xoa thong tin sinh vien theo ma id
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var std = await _context.Students.FindAsync(id);
            _context.Students.Remove(std);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}