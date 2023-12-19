using System.ComponentModel.DataAnnotations;

namespace PureCareHub_HospitalCare.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the doctor's first name.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the doctor's last name.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the phone number.")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the email.")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select working shift.")]
        [Display(Name = "Working Shift")]
        public ShiftType WorkingShift { get; set; }
        public DepartmentType Department { get; set; }
        public Gender DoctorGender { get; set; }

    }
}
