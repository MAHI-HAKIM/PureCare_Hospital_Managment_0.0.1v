using Microsoft.AspNetCore.Mvc;
using PureCareHub_HospitalCare.Data;
using PureCareHub_HospitalCare.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using PureCareHub_HospitalCare.Models;
using Microsoft.AspNetCore.Hosting;

namespace PureCareHub_HospitalCare.Controllers
{
	public class DepartmentController : Controller
	{
		private readonly ApplicationDBContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DepartmentController(ApplicationDBContext dbContext,
                                    IWebHostEnvironment webHostEnvironment)
		{
			_dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
		{
			var objDepartment = _dbContext.Departments.ToList();
			var objDoctor = _dbContext.doctors.ToList();    

			DepartmentViewModel viewModel = new DepartmentViewModel { 
				DepartmentList = objDepartment,
				DoctorList = objDoctor
			.Select(doctor => new SelectListItem
			{
				Value = doctor.Id.ToString(),
				Text = $"{doctor.FirstName} {doctor.LastName}"
			}).ToList()

			};

			return View(viewModel);
		}
		
		[HttpPost]
		public IActionResult Create(DepartmentViewModel model)
		{
			if(ModelState.IsValid)
			{
				string uniqueFileName = ProccessUploadedFile(model);

				Department department = new Department
				{
					DepartmentName = model.DepartmentName,
					DepartmentDescription = model.DepartmentDescription,
					PhotoPath = uniqueFileName
				};
				_dbContext.Add(department);
				_dbContext.SaveChanges();
				TempData["SuccessDep"] = "Successfully added a Department";
				return RedirectToAction("Index");
			}
			return View();
		}
        private string ProccessUploadedFile(DepartmentViewModel model)
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
		public IActionResult Edit(int ? depID) 
		{
			if (depID == null || depID == 0)
			{
				return NotFound();
			}
			Department? department = _dbContext.Departments.Find(depID);
			if (department == null)
			{
				return NotFound();
			}
			return View(department);
		}

		[HttpPost]
		public IActionResult Edit(DepartmentViewModel model)
		{
			if (ModelState.IsValid)
			{
				// Retrieve the existing department from the database
				Department existingDepartment = _dbContext.Departments.Find(model.Id);

				if (existingDepartment == null)
				{
					return NotFound(); // Handle the case where the department is not found
				}

				// Update the properties of the existing department with the new values
				existingDepartment.DepartmentName = model.DepartmentName;
				existingDepartment.DepartmentDescription = model.DepartmentDescription;

				// Update the department in the database
				_dbContext.Update(existingDepartment);
				_dbContext.SaveChanges();
				TempData["SuccessDep"] = "Successfully Updated a department";
				return RedirectToAction("Index"); // Redirect to the list of departments
			}

			return View(model); // If ModelState is not valid, return to the edit view with validation errors
		}

		public IActionResult Delete(int? ID)
		{
			Department doctorFromDb = _dbContext.Departments.Find(ID);

			if (doctorFromDb == null)
			{
				return NotFound();
			}

			_dbContext.Remove(doctorFromDb);
			_dbContext.SaveChanges();

			TempData["SuccessDep"] = "Successfully Deleted a Department";

			return RedirectToAction("Index");
		}
	}
}
