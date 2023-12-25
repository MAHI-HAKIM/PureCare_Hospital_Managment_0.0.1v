using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PureCareHub_HospitalCare.Data;
using PureCareHub_HospitalCare.Models;
using PureCareHub_HospitalCare.Models.Service;
using PureCareHub_HospitalCare.ViewModels;
using System.Security.Claims;

namespace PureCareHub_HospitalCare.Controllers
{
    public class MedicalHistoryController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
		private readonly IDocRepository _docRepository;


		public MedicalHistoryController(ApplicationDBContext dbContext,
										IDocRepository docRepository)
        {
            _dbContext = dbContext;
            _docRepository = docRepository;
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
            var appointedDoc = _dbContext.doctors.Find(appoinment.DoctorId);
			var departmentName = _docRepository.GetDepartmentName(appointedDoc.depID);

			MedicalHistoryViewModel medicalHistoryDetail = new MedicalHistoryViewModel()
            {
                appointments = appoinment,
                appointedDoctor = appointedDoc,
                DepartmentName = departmentName
            };
			if (medicalHistoryDetail == null)
			{
				return NotFound();
			}
			return View(medicalHistoryDetail);
		}
	}
}


