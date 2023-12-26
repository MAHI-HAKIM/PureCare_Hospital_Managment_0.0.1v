using System.ComponentModel.DataAnnotations;

namespace PureCareHub_HospitalCare.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the Departments name.")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; } = string.Empty;
        [Display(Name = "Department Description")]
        public string DepartmentDescription { get; set; } = string.Empty;

        public string PhotoPath { get; set; } = string.Empty;

    }
}
