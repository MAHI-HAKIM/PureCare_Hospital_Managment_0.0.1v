using Microsoft.AspNetCore.Mvc;
using PureCareHub_HospitalCare.Models;
using PureCareHub_HospitalCare.Data;
using PureCareHub_HospitalCare.ViewModels;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

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
        public IActionResult Create(Doctor doctor, DoctorRegistrationViewModel model)
        {
            if(ModelState.IsValid)
            {
                string uniqueFilename = "";

                if(model.Photo != null) {
                   string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                   uniqueFilename = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFilename);
                    model.Photo.CopyTo(new FileStream(filePath,FileMode.Create));

                    doctor.PhotoPath = uniqueFilename;
                }
                _dbContext.Add(doctor);
                _dbContext.SaveChanges();
                TempData["Success"] = "Successfully created a Doctor";
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ViewResult Detail(DoctorRegistrationViewModel model)
        {
            return View();
        }
    }
}
