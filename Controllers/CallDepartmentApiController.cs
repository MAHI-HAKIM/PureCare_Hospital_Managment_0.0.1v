using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PureCareHub_HospitalCare.Models;
using PureCareHub_HospitalCare.ViewModels;

namespace PureCareHub_HospitalCare.Controllers
{
	[Authorize(Roles = "Admin")]
	public class CallDepartmentApiController : Controller
    {
     
		public async Task<IActionResult> List()
        {
            List<Department> departmentList = new List<Department>();
            HttpClient client = new HttpClient();

            var response = await client.GetAsync("https://localhost:7078/api/DepartmentApi");
            var jsonResponse = await response.Content.ReadAsStringAsync();

            departmentList = JsonConvert.DeserializeObject<List<Department>>(jsonResponse);

            DepartmentViewModel viewModel = new DepartmentViewModel()
            {
                DepartmentList = departmentList
            };

            return View(viewModel);
        }


		[HttpPost]
		public async Task<IActionResult> Create(Department newDepartment)
		{
			if (!ModelState.IsValid)
			{
				// If the model state is not valid, return to the create view with validation errors
				return View(newDepartment);
			}

			HttpClient client = new HttpClient();

			// Send a POST request to create a new department
			var response = await client.PostAsJsonAsync("https://localhost:7078/api/DepartmentApi", newDepartment);

			if (response.IsSuccessStatusCode)
			{
				TempData["SuccessDep"] = "Successfully Added a department";
				return RedirectToAction("List");
			}
			else
			{
				// Handle the case where the creation request fails
				return View(newDepartment);
			}
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			HttpClient client = new HttpClient();

			// Fetch the department details using the department ID
			var response = await client.GetAsync($"https://localhost:7078/api/DepartmentApi/{id}");
			var jsonResponse = await response.Content.ReadAsStringAsync();

			var department = JsonConvert.DeserializeObject<Department>(jsonResponse);

			if (department == null)
			{
				// Handle the case where the department is not found
				return NotFound();
			}

			// Pass the department to the view for editing
			return View(department);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Department updatedDepartment)
		{
			if (!ModelState.IsValid)
			{
				// If the model state is not valid, return to the edit view with validation errors
				return View(updatedDepartment);
			}

			HttpClient client = new HttpClient();

			// Send a PUT request to update the department
			var response = await client.PutAsJsonAsync($"https://localhost:7078/api/DepartmentApi/{updatedDepartment.Id}", updatedDepartment);

			if (response.IsSuccessStatusCode)
			{
				TempData["SuccessDep"] = "Successfully Updated a department";
				return RedirectToAction("List");
			}
			else
			{
				// Handle the case where the update request fails
				return View(updatedDepartment);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			HttpClient client = new HttpClient();

			// Send a DELETE request to delete the department with the specified id
			var response = await client.DeleteAsync($"https://localhost:7078/api/DepartmentApi/{id}");

			if (response.IsSuccessStatusCode)
			{
				TempData["SuccessDep"] = "Successfully Updated a department";
				return RedirectToAction("List");
			}
			else
			{
				// Handle the case where the deletion request fails
				// You may want to return an error response
				return StatusCode((int)response.StatusCode, new { success = false, message = "Failed to delete department" });
			}
		}
	}
}
