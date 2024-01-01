using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PureCareHub_HospitalCare.Models;
using PureCareHub_HospitalCare.ViewModels;
using System.Text.Json.Serialization;

namespace PureCareHub_HospitalCare.Controllers
{
    public class CallServiceAPIController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<Department> departmentList = new List<Department>();
            HttpClient client = new HttpClient();

            var response = await client.GetAsync("https://localhost:7078/api/ServiceAPI");
            var jsonResponse = await response.Content.ReadAsStringAsync();

            departmentList = JsonConvert.DeserializeObject<List<Department>>(jsonResponse);

            return View(departmentList);
        }
    }
}
