using Microsoft.AspNetCore.Mvc;
using PureCareHub_HospitalCare.Models;
using PureCareHub_HospitalCare.Data;
using PureCareHub_HospitalCare.ViewModels;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;
using PureCareHub_HospitalCare.Models.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Authorization;

namespace PureCareHub_HospitalCare.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DoctorController : Controller
    {
        //private readonly IDocRepository _docRepository;
        private readonly ApplicationDBContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDocRepository _docRepository;


        //private readonly 
        [Obsolete]
        public DoctorController(ApplicationDBContext dbContext,
                                IWebHostEnvironment webHostEnvironment,
                                IDocRepository docRepository)
        { 
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _docRepository = docRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var objDocotr = _dbContext.doctors.ToList();
            var doctorViewModels = new List<DoctorViewModel>();

			if (objDocotr == null || doctorViewModels == null)
			{
				Response.StatusCode = 404;
				return View("404ID", 404);
			}
			foreach (var doctor in objDocotr)
            {
                var departmentName = _docRepository.GetDepartmentName(doctor.depID);
                doctorViewModels.Add(new DoctorViewModel { doctor = doctor, DepartmentName = departmentName });
            }

            return View(doctorViewModels);
        }
        [HttpGet]
        public IActionResult List()
        {
            var objDocotr = _dbContext.doctors.ToList();
            var doctorViewModels = new List<DoctorViewModel>();

			if (objDocotr == null || doctorViewModels == null)
			{
				Response.StatusCode = 404;
				return View("404ID", 404);
			}

			foreach (var doctor in objDocotr)
            {
                var departmentName = _docRepository.GetDepartmentName(doctor.depID);
                doctorViewModels.Add(new DoctorViewModel { doctor = doctor, DepartmentName = departmentName });
            }

            return View(doctorViewModels);
        }

        [HttpGet]
        public ViewResult Create()
        {
            var viewModel = new DoctorRegistrationViewModel {
                DepartmentsList = new SelectList(_dbContext.Departments.ToList(), "Id", "DepartmentName"),
            };

			if (viewModel == null)
			{
				Response.StatusCode = 404;
				return View("404ID", 404);
			}

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Create(DoctorRegistrationViewModel model)
        {

            if (model.WorkingDays == null || !model.WorkingDays.Any())
            {
                model.DepartmentsList = new SelectList(_dbContext.Departments.ToList(), "Id", "DepartmentName");
                ModelState.AddModelError(nameof(DoctorEditViewModel.WorkingDays), "Please select at least one day.");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                string uniqueFilename = ProccessUploadedFile(model);


				Doctor doctor = new Doctor
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    WorkingShift = model.WorkingShift,
                    depID = model.SelectedDepartmentId,
                    DoctorGender = model.DoctorGender,
                    PhotoPath = uniqueFilename
                };

                // Save the doctor information
                _dbContext.Add(doctor);
                _dbContext.SaveChanges();

                // Retrieve the newly created doctor ID
                int doctorId = doctor.Id;

                // Save the doctor's schedule
                ICollection<DayOfWeek> selectedDays = model.WorkingDays ?? new List<DayOfWeek>();

                foreach (var dayOfWeek in selectedDays)
                {
                    DoctorsSchedule schedule = new DoctorsSchedule
                    {
                        DoctorId = doctorId,
                        DayOfWeek = dayOfWeek,
                        StartTime = model.StartTime, // You need to modify this based on your ViewModel
                        EndTime = model.EndTime, // You need to modify this based on your ViewModel
                        IsAvailable = true,
                    };

                    _dbContext.Add(schedule);
                }

			

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
        private string ProccessUploadedFile(DoctorEditViewModel model)
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

			if (doctordb == null)
			{
				Response.StatusCode = 404;
				return View("404ID", 404);
			}

			ICollection<DoctorsSchedule>? doctorSchedulesList = _dbContext.DoctorSchedules
            .Where(schedule => schedule.DoctorId == docid)
            .ToList();

            // Find any schedule of that doctor
            DoctorsSchedule firstSchedule = _dbContext.DoctorSchedules.Where(schedule => schedule.DoctorId == docid).FirstOrDefault();

            DoctorEditViewModel viewModel = new DoctorEditViewModel()
            {
                doctor = doctordb,
                ExistingPhotoPath = doctordb.PhotoPath,
                schedules = doctorSchedulesList,
                SelectedDepartmentId = doctordb.depID,
                DepartmentsList = new SelectList(_dbContext.Departments.ToList(), "Id", "DepartmentName"),
                doctorsSchedule = firstSchedule
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(DoctorEditViewModel model)
        {

            if (model.WorkingDays == null || !model.WorkingDays.Any())
            {
                ICollection<DoctorsSchedule>? doctorSchedulesList = _dbContext.DoctorSchedules
              .Where(schedule => schedule.DoctorId == model.doctor.Id)
                .ToList();

                model.schedules = doctorSchedulesList;
                model.DepartmentsList = new SelectList(_dbContext.Departments.ToList(), "Id", "DepartmentName");
                ModelState.AddModelError(nameof(DoctorEditViewModel.WorkingDays), "Please select at least one day.");
                return View(model);
            }


            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                
                Doctor doctor = _dbContext.doctors.Include(d => d.DoctorSchedules).FirstOrDefault(d => d.Id == model.doctor.Id);

                if (doctor == null)
                {
                    return NotFound();
                }

                // Update doctor information
                doctor.FirstName = model.doctor.FirstName;
                doctor.LastName = model.doctor.LastName;
                doctor.PhoneNumber = model.doctor.PhoneNumber;
                doctor.Email = model.doctor.Email;
                doctor.depID = model.SelectedDepartmentId;
                doctor.WorkingShift = model.doctor.WorkingShift;
                doctor.DoctorGender = model.doctor.DoctorGender;

                if (model.Photo != null)
                {
                    if (doctor.PhotoPath != null)
                    {
                        if(model.ExistingPhotoPath != null)
                        {
                            string filepath = Path.Combine(_webHostEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                            System.IO.File.Delete(filepath);
                        }
                        doctor.PhotoPath = ProccessUploadedFile(model);
                    }
                }

                ICollection<DayOfWeek> selectedDays = model.WorkingDays ?? new List<DayOfWeek>();

				if (model.WorkingDays == null || !model.WorkingDays.Any())
				{
					ICollection<DoctorsSchedule>? doctorSchedulesList = _dbContext.DoctorSchedules
				  .Where(schedule => schedule.DoctorId == model.doctor.Id)
					.ToList();

					model.schedules = doctorSchedulesList;
					model.DepartmentsList = new SelectList(_dbContext.Departments.ToList(), "Id", "DepartmentName");
					ModelState.AddModelError(nameof(DoctorEditViewModel.WorkingDays), "Please select at least one day.");
					return View(model);
				}

				var schedulesToRemove = doctor.DoctorSchedules.Where(d => d.DoctorId == model.doctor.Id).ToList();

                // Remove existing schedules
                foreach (var schedule in schedulesToRemove)
                {
                    _dbContext.Remove(schedule);
                }

				// Add new schedules
				foreach (var dayOfWeek in selectedDays)
                {
                        DoctorsSchedule newSchedule = new DoctorsSchedule
                        {
                            DoctorId = model.doctor.Id,
                            DayOfWeek = dayOfWeek,
                            StartTime = model.doctorsSchedule.StartTime,
                            EndTime = model.doctorsSchedule.EndTime,
                            IsAvailable = true,
                        };

                        _dbContext.Add(newSchedule);
                }
                _dbContext.Update(doctor);
                _dbContext.SaveChanges();
                TempData["Success"] = "Successfully Updated a Doctor";
                return RedirectToAction("List");
            }

            model.DepartmentsList = new SelectList(_dbContext.Departments.ToList(), "Id", "DepartmentName");

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

			if (doctordb == null)
			{
				Response.StatusCode = 404;
				return View("404ID", docid);
			}

			return View(doctordb);
        }
        [HttpPost,ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePost(int? ID)
        {
            Doctor doctorFromDb = _dbContext.doctors.Find(ID);


			if (doctorFromDb == null)
			{
				Response.StatusCode = 404;
				return View("404ID", ID);
			}

			_dbContext.Remove(doctorFromDb);
            _dbContext.SaveChanges();

            TempData["Success"] = "Successfully Deleted a Doctor";

            return RedirectToAction("List");
        }
    }
}
