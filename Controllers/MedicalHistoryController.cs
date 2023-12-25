using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PureCareHub_HospitalCare.Data;
using PureCareHub_HospitalCare.Models;
using PureCareHub_HospitalCare.ViewModels;
using System.Security.Claims;

namespace PureCareHub_HospitalCare.Controllers
{
    public class MedicalHistoryController : Controller
    {
        private readonly ApplicationDBContext _dbContext;

        public MedicalHistoryController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the logged-in user's ID
            var patient = _dbContext.patients.FirstOrDefault(p => p.UserId == userId);


            var appointments = _dbContext.appointments
               .Where(a => a.PatientId == patient.Id)
               .Include(a => a.AssociatedDoctor)
                .Include(a => a.Patient)
               .ToList();
            return View(appointments);
        }
        private int GetLoggedInPatientId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the logged-in user's ID

            // Retrieve the associated patient directly from the DbContext
            var patient = _dbContext.patients.FirstOrDefault(p => p.UserId == userId);

            return patient.Id; // Replace with your actual logic
        }
		[HttpGet]
		public IActionResult Detail(int? appointmentID)
		{
			if (appointmentID == null || appointmentID == 0)
			{
				return NotFound();
			}
			Appointment? appoinment = _dbContext.appointments.Find(appointmentID);
			if (appoinment == null)
			{
				return NotFound();
			}
			return View(appoinment);
		}
	}
}


