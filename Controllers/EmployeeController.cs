using HoTrungNguyenBTTH2.Data;
using HoTrungNguyenBTTH2.Models;
using HoTrungNguyenBTTH2.Models.process;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HoTrungNguyenBTTH2.Controllers
{
    public class EmployeeController : Controller
    {   
        //khai bao Dbcontext de lam viec voi database
         private readonly ApplicationDbContext _context;
         public EmployeeController (ApplicationDbContext context)
        {
            _context = context;
        }
         //khai bao class excelprocess trong employeecontroller
        private ExcelProcess _excelProcess = new ExcelProcess();
        //Tra ve View Index danh sach cac Employee
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employee.ToListAsync());
        }
        //Kiem tra Employee theo id co ton tai k?
        private bool EmployeeExists(string id)
        {
            return _context.Employee.Any( e => e.EmpID == id);
        }
        //Action Upload file excel len sever
        public async Task<IActionResult> Upload()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload (IFormFile file)
        {
            if(file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if(fileExtension != ".xls" && fileExtension !=".xlsx")
                {
                    ModelState.AddModelError("","Please choose excel file to upload!");
                }
                else
                {
                    //rename file when upload to sever
                    var fileName = DataTime.Now.ToShortTimeString()+ fileExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory()+ "/Uploads/Excels", fileName);
                    var fileLocation = new FileInfo(filePath).ToString();
                    using (var stream  = new FileStream(filePath, FileMode.Create))
                    {
                        //save file to sever
                        await file.CopyToAsync(stream);
                        //read data from file and write to database
                        var dt = _excelProcess.ExceltoDataTable(fileLocation);
                        //using for loop to read data form dt
                        for(int i = 0;i < dt.Rows.Count;i++)
                        {
                            //create a new Employee object 
                            var emp = new Employee();
                            //set values for attributes
                            emp.EmpID= dt.Rows[i][0].ToString();
                            emp.EmpName = dt.Rows[i][1].ToString();
                        }

                    }                    

                }
            }
            return View();
        }
       

    }   
}