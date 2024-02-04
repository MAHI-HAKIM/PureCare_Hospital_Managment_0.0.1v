using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PureCareHub_HospitalCare.Models;
using System.Diagnostics;

namespace PureCareHub_HospitalCare.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
		{
			_logger = logger;
            _localizer = localizer;

        }

        [HttpGet]
		public IActionResult Index()
		{
            @ViewData["Welcome"] = _localizer["Welcome"];
            @ViewData["takeAppointment"] = _localizer["takeAppointment"];
            @ViewData["Heropage"] = _localizer["Heropage"];
            @ViewData["WhyChooseUs"] = _localizer["WhyChooseUs"];
            @ViewData["ComprehensiveCare"] = _localizer["ComprehensiveCare"];
            @ViewData["MedicalTechnology"] = _localizer["MedicalTechnology"];
            @ViewData["HealthcareProfessionals"] = _localizer["HealthcareProfessionals"];
            @ViewData["PatientCenteredApproach"] = _localizer["PatientCenteredApproach"];
            @ViewData["CommitmentToExcellence"] = _localizer["CommitmentToExcellence"];

            ViewData["PatientCentricCare_Title"] = _localizer["PatientCentricCare_Title"];
            ViewData["PatientCentricCare_Description"] = _localizer["PatientCentricCare_Description"];

            ViewData["QualityAssurance_Title"] = _localizer["QualityAssurance_Title"];
            ViewData["QualityAssurance_Description"] = _localizer["QualityAssurance_Description"];

            ViewData["DedicatedSupportTeam_Title"] = _localizer["DedicatedSupportTeam_Title"];
            ViewData["DedicatedSupportTeam_Description"] = _localizer["DedicatedSupportTeam_Description"];

            return View();
		}
		[Authorize(Roles = "Admin")]
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

    }
}