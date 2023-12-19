using PureCareHub_HospitalCare.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace PureCareHub_HospitalCare.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }


        // Use ICollection for related data
        //public ICollection<Appointment>? Appointments { get; set; }
        //public ICollection<MedicalHistory>? MedicalHistories { get; set; }

        // Foreign key to link with ApplicationUser
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
    }
}
