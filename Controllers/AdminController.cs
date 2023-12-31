using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PureCareHub_HospitalCare.Areas.Identity.Data;
using PureCareHub_HospitalCare.Data;
using PureCareHub_HospitalCare.Models;
using PureCareHub_HospitalCare.ViewModels;

namespace PureCareHub_HospitalCare.Controllers
{
    [Authorize]
	public class AdminController : Controller
	{
		private readonly ApplicationDBContext dbContext;
		private readonly UserManager<ApplicationUser> _userManager;

		public AdminController(ApplicationDBContext _dbContext,
								UserManager<ApplicationUser> userManager)
		{ 
			dbContext = _dbContext;
			_userManager = userManager;
		}

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DashBoard()
		{
			var patients = dbContext.patients.ToList(); // Make sure to use the correct property name

			// Check if the patient is found
			if (patients == null)
			{
				Response.StatusCode = 404;
				return View("404ID",404);
			}
			var viewModel = new PatientsListViewModel
			{
				Patients = patients,
                Doctorscount = dbContext.doctors.Count(),
                DepartmentsCount = dbContext.Departments.Count(),
                FullAppointmentCount = dbContext.appointments.Count(),
                patientCount = dbContext.patients.Count()

            };			
			await viewModel.PopulateEmailsAndCountsAsync(_userManager, dbContext);

			return View(viewModel);
		}

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
					Response.StatusCode = 404;
					return View("404ID", userId);
			}

            // Find and delete the related patient
            var patient = await dbContext.patients.SingleOrDefaultAsync(p => p.UserId == userId);
            if (patient != null)
            {
                dbContext.patients.Remove(patient);
                await dbContext.SaveChangesAsync();
            }

            // Find and delete the Identity user
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["Success"] = "You have successfully made an appointment";
                    return View("DashBoard");
                }
                else
                {
                    // Handle errors if deletion fails
                    return BadRequest(result.Errors.Select(error => error.Description).ToList());
                }
            }
            return NotFound("User not found");
        }
    }
}