using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PureCareHub_HospitalCare.Data;
using PureCareHub_HospitalCare.Models;
using PureCareHub_HospitalCare.Models.Service;
using PureCareHub_HospitalCare.ViewModels;
using System.Numerics;
using System.Security.Claims;

namespace PureCareHub_HospitalCare.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IDocRepository _docRepository;
        
        public AppointmentController(ApplicationDBContext dBContext,
                                     IDocRepository docRepository)
        {
            _dbContext = dBContext;
            _docRepository = docRepository;

        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the logged-in user's ID

            // Retrieve the associated patient directly from the DbContext
            var patient = _dbContext.patients.FirstOrDefault(p => p.UserId == userId);

            // Check if the patient is found
            if (patient == null)
            {
                // Handle the case where the patient is not found
                // You might want to redirect to an error page or take some other action
                return NotFound();
            }

            var doctorsList = _dbContext.doctors.ToList();

            var viewModel = new AppointmentRegistationViewModel
            {
                PatientId = patient.Id,
                patientFirstname = patient.FirstName,
                patientLastName = patient.LastName,
                AppointmentDate = DateTime.Now,
                ListofDepartments = new SelectList(_dbContext.Departments.ToList(), "Id", "DepartmentName")
            };

            return View(viewModel);

          
        }

		[ValidateAntiForgeryToken]
		[HttpPost]
        public IActionResult Index(AppointmentRegistationViewModel model)
        {
            //model.ListofDepartments = new SelectList(_dbContext.Departments.ToList(), "Id", "DepartmentName");
            //foreach (var error in ModelState["SelectedDepartmentId"].Errors)
            //{
            //    Console.WriteLine($"ModelState Error for SelectedDepartmentId: {error.ErrorMessage}");
            //}
            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    Console.WriteLine($"Model Error for {entry.Key}: {error.ErrorMessage}");
                }
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction("Success", model);
            }

            model.ListofDepartments = new SelectList(_dbContext.Departments.ToList(), "Id", "DepartmentName");

            return View(model);
        }
        [HttpGet]
        public IActionResult Success(AppointmentRegistationViewModel model)
        {
            model.assingedDoctor = _dbContext.doctors.Find(model.DoctorId);
            model.departmentName = _docRepository.GetDepartmentName(model.SelectedDepartmentId);
            return View(model);
        }

        [HttpPost, ActionName("Success")]
        public IActionResult RegisterSuccess(AppointmentRegistationViewModel model)
        {
            if (model == null)
            {
                return NotFound();
            }

            Appointment appointment = new Appointment
            {
                AppointmentDate = model.AppointmentDate,
                patientFirstname = model.patientFirstname,
                patientLastName = model.patientLastName,
                patientContactNumber = model.patientContactNumber,
                PatientId = model.PatientId,
                DoctorId = model.DoctorId,
                AdditionalInfo = model.AdditionalInfo
            };

            _dbContext.Add(appointment);
            _dbContext.SaveChanges();
            ViewData["AppointmentDetails"] = appointment;
            ViewData["PageTitle"] = "Appointment Detail";
            TempData["Success"] = "You have successfully made an appointment";

            // Redirect to the "Index" action of the "Home" controller
            return RedirectToAction("Index", "Home");
        }
        public JsonResult getDoctorsByDepartmentType(int  departmentID)
        {
            return Json(_dbContext.doctors.Where(d => d.depID == departmentID).ToList());
        }
    }
}
