using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
        [Authorize(Roles="Admin")]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the logged-in user's ID

            // Retrieve the associated patient directly from the DbContext
            var patient = _dbContext.patients.FirstOrDefault(p => p.UserId == userId);

            // Check if the patient is found
            if (patient == null)
            {
                Response.StatusCode = 404;
                return View("404ID", userId);
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
        private AppointmentRegistationViewModel populateInfo(int DocID)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the logged-in user's ID

            // Retrieve the associated patient directly from the DbContext
            var patient = _dbContext.patients.FirstOrDefault(p => p.UserId == userId);

            // Check if the patient is found


            ICollection<DoctorsSchedule>? doctorsSchedules = _dbContext.DoctorSchedules.Where(schedule => schedule.DoctorId == DocID).ToList();
            DoctorsSchedule firstSchedule = _dbContext.DoctorSchedules.Where(schedule => schedule.DoctorId == DocID).FirstOrDefault();


            if (doctorsSchedules == null || firstSchedule == null)
            {
                return null;
            }
            var timeSlot = GetTimeSlots(firstSchedule.StartTime, firstSchedule.EndTime, 60);



            AppointmentRegistationViewModel viewModel = new AppointmentRegistationViewModel()
            {
                patientFirstname = patient.FirstName,
                patientLastName = patient.LastName,
                PatientId = patient.Id,
                DoctorId = DocID,
                assingedDoctor = _dbContext.doctors.Find(DocID),
                schedules = doctorsSchedules,
                doctorsSchedule = firstSchedule,
                stratTime = firstSchedule.StartTime.ToString("hh:mm tt"),
                endTime = firstSchedule.EndTime.ToString("hh:mm tt"),
                workingHours = timeSlot
            };

            return viewModel;
        }
        public List<string> GetTimeSlots(DateTime doctorStartTime, DateTime doctorEndTime, int timeSlotIntervalInMinutes)
        {
            List<string> timeSlots = new List<string>();
            DateTime currentTime = doctorStartTime;

            while (currentTime < doctorEndTime)
            {
                timeSlots.Add(currentTime.ToString("h:mm tt")); // Format time as "1:30 PM"
                currentTime = currentTime.AddMinutes(timeSlotIntervalInMinutes);
            }

            return timeSlots;
        }
        public JsonResult getDoctorsByDepartmentType(int departmentID)
        {
            return Json(_dbContext.doctors.Where(d => d.depID == departmentID).ToList());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Index(AppointmentRegistationViewModel model)
        {
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
        [Authorize(Roles = "Admin")]
        public IActionResult List()
        {
            var appointments = _dbContext.appointments.Include(a => a.AssociatedDoctor).Include(a => a.Patient).ToList();

            if (appointments == null)
            {
                Response.StatusCode = 404;
                return View("404ID");
            }
            return View(appointments);
        }

        [HttpGet]
        public IActionResult Create(int DocID)
        {
            AppointmentRegistationViewModel viewModel = populateInfo(DocID);

            if (viewModel == null)
            {
                Response.StatusCode = 404;
                return View("404ID", DocID);
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(AppointmentRegistationViewModel model)
        {
            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    Console.WriteLine($"Model Error for {entry.Key}: {error.ErrorMessage}");
                }
            }
            if (ModelState.IsValid)
            {
                Appointment appointment = new Appointment()
                {
                    patientFirstname = model.patientFirstname,
                    patientLastName = model.patientLastName,
                    AppointmentDate = model.AppointmentDate,
                    PatientId = model.PatientId,
                    patientContactNumber = model.patientContactNumber,
                    DoctorId = model.DoctorId,
                    StartTime = model.AppointmentDate + model.AppointmentTime,
                    EndTime = model.AppointmentDate + model.AppointmentTime.Add(TimeSpan.FromMinutes(60)),
                    AdditionalInfo = model.AdditionalInfo,
                };

                var selectedDateTime = model.AppointmentDate + model.AppointmentTime;
                var appointmentEndTime = selectedDateTime.Add(TimeSpan.FromHours(1));

                // Check if there is any existing appointment for the selected time slot
                var existingAppointment = _dbContext.appointments.FirstOrDefault(a =>
                    a.DoctorId == model.DoctorId &&
                    a.AppointmentDate == model.AppointmentDate &&
                    ((selectedDateTime >= a.StartTime && selectedDateTime < a.EndTime) ||
                     (appointmentEndTime > a.StartTime && appointmentEndTime <= a.EndTime)));

                if (existingAppointment != null)
                {
                    // Doctor is not available during the selected time
                    // Handle the case where the selected time is not available
                    model = populateInfo(model.DoctorId);
                    ModelState.AddModelError("AppointmentTime", "Doctor is not available during the selected time.");
                    // You might also want to provide this information to the user in the view
                    return View(model);
                }
                _dbContext.Add(appointment);
                _dbContext.SaveChanges();
                ViewData["AppointmentDetails"] = appointment;
                ViewData["PageTitle"] = "Appointment Detail";
                TempData["Success"] = "You have successfully made an appointment";

                // Redirect to "MedicalHistory/Index" for others (assuming they are patients)
                return RedirectToAction("Index", "MedicalHistory");

            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminAppointment(int DocID)
        {
            AppointmentRegistationViewModel viewModel = populateInfo(DocID);

            if (viewModel == null)
            {
                Response.StatusCode = 404;
                return View("404ID", viewModel.DoctorId);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminAppointment(AppointmentRegistationViewModel model)
        {
            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    Console.WriteLine($"Model Error for {entry.Key}: {error.ErrorMessage}");
                }
            }
            if (ModelState.IsValid)
            {
                Appointment appointment = new Appointment
                {
                    AppointmentDate = model.AppointmentDate,
                    patientFirstname = model.patientFirstname,
                    patientLastName = model.patientLastName,
                    patientContactNumber = model.patientContactNumber,
                    //PatientId = model.PatientId,
                    DoctorId = model.DoctorId,
                    AdditionalInfo = model.AdditionalInfo
                };

                appointment.PatientId = null;

                _dbContext.Add(appointment);
                _dbContext.SaveChanges();
                ViewData["AppointmentDetails"] = appointment;
                ViewData["PageTitle"] = "Appointment Detail";
                TempData["Success"] = "You have successfully made an appointment";

                return RedirectToAction("List");
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

            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                // Redirect to "Appointment/List" for Admin
                return RedirectToAction("List", "Appointment");
            }
            else
            {
                // Redirect to "MedicalHistory/Index" for others (assuming they are patients)
                return RedirectToAction("Index", "MedicalHistory");
            }
        }
    }
}
