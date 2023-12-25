using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PureCareHub_HospitalCare.Areas.Identity.Data;
using PureCareHub_HospitalCare.Data;
using PureCareHub_HospitalCare.ViewModels;

namespace PureCareHub_HospitalCare.Controllers
{
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
		public async Task<IActionResult> PatientList()
		{
			var patients = dbContext.patients.ToList(); // Make sure to use the correct property name

			var viewModel = new PatientsListViewModel
			{
				Patients = patients,
			};

			
			await viewModel.PopulateEmailsAndCountsAsync(_userManager, dbContext);

			return View(viewModel);
		}
	}
}
