using Microsoft.AspNetCore.Mvc.Rendering;
using PureCareHub_HospitalCare.Models;
using System.ComponentModel.DataAnnotations;

namespace PureCareHub_HospitalCare.ViewModels
{
	public class DepartmentViewModel
	{
		public int Id { get; set; }
		public List<Department>? DepartmentList { get; set; }
		public List<SelectListItem>? DoctorList { get; set;}

		[Required(ErrorMessage = "Please enter the Departments name.")]
		[Display(Name = "Department Name")]
		public string DepartmentName { get; set; } = string.Empty;
		[Display(Name = "Department Description")]

		public string DepartmentDescription { get; set; } = string.Empty;
        public IFormFile? Photo { get; set; }
    }
}
