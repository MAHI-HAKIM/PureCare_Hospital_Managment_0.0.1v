using Microsoft.AspNetCore.Mvc;
using PureCareHub_HospitalCare.Models;
using PureCareHub_HospitalCare.Data;
using PureCareHub_HospitalCare.ViewModels;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;
using PureCareHub_HospitalCare.Models.Service;
using Microsoft.AspNetCore.Hosting;

namespace PureCareHub_HospitalCare.Controllers
{
    public class DoctorController : Controller
    {
        //private readonly IDocRepository _docRepository;
        private readonly ApplicationDBContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;


        //private readonly 
        [Obsolete]
        public DoctorController(ApplicationDBContext dbContext,
                                IWebHostEnvironment webHostEnvironment)
        { 
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var objDocotr = _dbContext.doctors.ToList();
            return View(objDocotr);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DoctorRegistrationViewModel model)
        {
            if(ModelState.IsValid)
            {
                string uniqueFilename = ProccessUploadedFile(model);

                Doctor doctor = new Doctor
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    WorkingShift = model.WorkingShift,
                    Department = model.Department,
                    DoctorGender = model.DoctorGender,
                    PhotoPath = uniqueFilename
                };

                _dbContext.Add(doctor);
                _dbContext.SaveChanges();
                TempData["Success"] = "Successfully created a Doctor";
                return RedirectToAction("List");
            }
            return View();
        }

        private string ProccessUploadedFile(DoctorRegistrationViewModel model)
        {
            string uniqueFilename = "";

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFilename = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFilename);
                model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            return uniqueFilename;
        }

        [HttpGet]
        public IActionResult Edit(int? docid)
        {
            if (docid == null || docid == 0)
            {
                return NotFound();
            }

            Doctor? doctordb = _dbContext.doctors.Find(docid);

            //category? categoryfromdb1 = _dbcontext.categories.firstordefault(u=>u.categoryid == id);
            ////category? categoryfromdb2 = _dbcontext.categories.where(u=>u.categoryid==id).firstordefault();

            if (doctordb == null)
            {
                return NotFound();
            }
           
            return View(doctordb);
        }

        [HttpPost]
        public IActionResult Edit(DoctorEditViewModel model)
        {
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            if (ModelState.IsValid)
            {

                Doctor doctor = _dbContext.doctors.Find(model.Id);

                if (doctor == null)
                {
                    return NotFound();
                }

                doctor.FirstName = model.FirstName;
                doctor.LastName = model.LastName;
                doctor.PhoneNumber = model.PhoneNumber;
                doctor.Email = model.Email;
                doctor.WorkingShift = model.WorkingShift;
                doctor.Department = model.Department;
                doctor.DoctorGender = model.DoctorGender;

                if(model.Photo != null)
                {
                    if (doctor.PhotoPath != null)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath,
                                            "images", doctor.PhotoPath);
                        try
                        {
                            System.IO.File.Delete(filePath);
                        }
                        catch (Exception ex)
                        {
                            // Log or handle the exception appropriately
                            Console.WriteLine($"Error deleting file: {ex.Message}");
                        }
                    }
                    doctor.PhotoPath = ProccessUploadedFile(model);
                }
               
                _dbContext.Update(doctor);
                _dbContext.SaveChanges();
                TempData["Success"] = "Successfully Updated a Doctor";
                return RedirectToAction("List");
            }
            return View();
        }
        public IActionResult List()
        {
            var model = _dbContext.doctors.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? docid)
        {
            if (docid == null || docid == 0)
            {
                return NotFound();
            }

            Doctor doctordb = _dbContext.doctors.Find(docid);

            //category? categoryfromdb1 = _dbcontext.categories.firstordefault(u=>u.categoryid == id);
            ////category? categoryfromdb2 = _dbcontext.categories.where(u=>u.categoryid==id).firstordefault();

            if (doctordb == null)
            {
                return NotFound();
            }

            return View(doctordb);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int? ID)
        {
            Doctor doctorFromDb = _dbContext.doctors.Find(ID);

            if(doctorFromDb == null)
            {
                return NotFound();
            }

            _dbContext.Remove(doctorFromDb);
            _dbContext.SaveChanges();

            TempData["Success"] = "Successfully Deleted a Doctor";

            return RedirectToAction("List");
        }
    }
}
