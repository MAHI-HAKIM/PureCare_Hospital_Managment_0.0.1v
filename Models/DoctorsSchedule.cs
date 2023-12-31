using System.ComponentModel.DataAnnotations;

namespace PureCareHub_HospitalCare.Models
{
    public class DoctorsSchedule
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the day of the week.")]
        [Display(Name = "Day of the Week")]
        public DayOfWeek DayOfWeek { get; set; }

        [Required(ErrorMessage = "Please enter the start time.")]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Please enter the end time.")]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        // Foreign key to associate with the Doctor entity
        public int DoctorId { get; set; }

        // Navigation property to represent the relationship with doctors
        public Doctor? doctor { get; set; }
        public bool IsAvailable { get; set; } // New property for availability


    }
}

