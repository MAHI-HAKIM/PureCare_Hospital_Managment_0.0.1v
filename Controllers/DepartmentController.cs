using Microsoft.AspNetCore.Mvc;
using PureCareHub_HospitalCare.Data;
using PureCareHub_HospitalCare.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using PureCareHub_HospitalCare.Models;

namespace PureCareHub_HospitalCare.Controllers
{
	public class DepartmentController : Controller
	{
		private readonly ApplicationDBContext _dbContext;

		public DepartmentController(ApplicationDBContext dbContext)
		{
			_dbContext = dbContext;
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
				Department department = new Department
				{
					DepartmentName = model.DepartmentName,
					DepartmentDescription = model.DepartmentDescription,
				};
				_dbContext.Add(department);
				_dbContext.SaveChanges();
				TempData["SuccessDep"] = "Successfully added a Department";
				return RedirectToAction("Index");
			}
			return View();
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
