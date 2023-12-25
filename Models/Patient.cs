using Microsoft.AspNetCore.Identity;
using PureCareHub_HospitalCare.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureCareHub_HospitalCare.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Please enter the First name.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Please enter the Last name.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        // Use ICollection for related data
        public ICollection<Appointment>? Appointments { get; set; }
        //public ICollection<MedicalHistory>? MedicalHistories { get; set; }

        // Foreign key to link with ApplicationUser
        public string UserId { get; set; } 
        public ApplicationUser? User { get; set; }
    }
}
