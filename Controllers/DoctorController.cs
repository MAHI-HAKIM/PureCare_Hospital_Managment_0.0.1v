using Microsoft.AspNetCore.Mvc;

namespace PureCareHub_HospitalCare.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
