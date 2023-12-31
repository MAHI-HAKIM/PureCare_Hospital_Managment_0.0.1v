using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging.Signing;
using PureCareHub_HospitalCare.Models;
using System.ComponentModel.DataAnnotations;

namespace PureCareHub_HospitalCare.ViewModels
{
    public class DoctorEditViewModel
    {
        public Doctor ?doctor { get; set; }
        public ICollection<DoctorsSchedule>? schedules { get; set; }

        [Required(ErrorMessage = "Please select the department.")]
        [Display(Name = "Department")]
        public int SelectedDepartmentId { get; set; }
        public SelectList? DepartmentsList { get; set; }
        public DoctorsSchedule ?doctorsSchedule { get; set; }
        public IFormFile? Photo { get; set; }
        public string ?ExistingPhotoPath {  get; set; }

        [Required(ErrorMessage = "Please select the working days.")]
        [Display(Name = "Working Days")]
        public ICollection<DayOfWeek>? WorkingDays { get; set; }
    }
}
