using PureCareHub_HospitalCare.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace PureCareHub_HospitalCare.Models
{
    public class Patient
    {
        [Key]
        public int patientId { get; set; }

        [Required(ErrorMessage = "Please enter the patient's first name.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the patient's last name.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }= string.Empty;

        [Required(ErrorMessage = "Please enter the patient's contact number.")]
        [Display(Name = "Contact Number")]
        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the patient's date of birth.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        // Use ICollection for related data
        //public ICollection<Appointment>? Appointments { get; set; }
        //public ICollection<MedicalHistory>? MedicalHistories { get; set; }

        // Foreign key to link with ApplicationUser
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
    }
}
