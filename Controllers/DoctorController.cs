using Microsoft.AspNetCore.Mvc;
using PureCareHub_HospitalCare.Models;
using PureCare.Doctors.IRepository;
using PureCare.Doctors.Model;
using PureCareHub_HospitalCare.Data;

namespace PureCareHub_HospitalCare.Controllers
{
    public class DoctorController : Controller
    {
        //private readonly IDocRepository _docRepository;
        private readonly ApplicationDBContext _dbContext;
        public DoctorController(ApplicationDBContext dbContext) 
        { 
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Doctor doctor)
        {
            if(ModelState.IsValid)
            {
                _dbContext.Add(doctor);
                _dbContext.SaveChanges();
                TempData["Success"] = "Successfully created a Doctor";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
