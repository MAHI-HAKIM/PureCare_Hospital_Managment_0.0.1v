using Microsoft.AspNetCore.Mvc.Rendering;
using PureCareHub_HospitalCare.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace PureCareHub_HospitalCare.ViewModels
{
    public class DoctorRegistrationViewModel
    {

        [Required(ErrorMessage = "Please enter the doctor's first name.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter the doctor's last name.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter the phone number.")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter the email.")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string ?Email { get; set; }

        [Required(ErrorMessage = "Please select working shift.")]
        [Display(Name = "Working Shift")]
        public ShiftType WorkingShift { get; set; }

        [Required(ErrorMessage = "Please select the department.")]
        [Display(Name = "Department")]
        public int SelectedDepartmentId { get; set; }
        public SelectList? DepartmentsList { get; set; }
        public Gender DoctorGender { get; set; }
        public IFormFile? Photo { get; set; }

        [Required(ErrorMessage = "Please select the working days.")]
        [Display(Name = "Working Days")]
        public ICollection<DayOfWeek> ?WorkingDays { get; set; }

        [Required(ErrorMessage = "Please enter the start time.")]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Please enter the end time.")]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        public List<SelectListItem> DayOfWeekSelectList
        {
            get
            {
                return Enum.GetValues(typeof(DayOfWeek))
                           .Cast<DayOfWeek>()
                           .Select(day => new SelectListItem
                           {
                               Value = day.ToString(),
                               Text = Enum.GetName(typeof(DayOfWeek), day)
                           })
                           .ToList();
            }
        }
    }
}
