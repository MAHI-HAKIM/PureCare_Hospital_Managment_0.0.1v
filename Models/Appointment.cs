using System.ComponentModel.DataAnnotations;

namespace PureCareHub_HospitalCare.Models
{
    public class Appointment
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Please enter the appointment date.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Please enter the patient's name.")]
        [Display(Name = "First Name")]
        public string patientFirstname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the patient's name.")]
        [Display(Name = "Last Name")]
        public string patientLastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the patient's contact number.")]
        [Display(Name = "Patient Contact Number")]
        [DataType(DataType.PhoneNumber)]
        public string patientContactNumber { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Please enter the appointment start time.")]
        //[DataType(DataType.DateTime)]
        //[Display(Name = "Appointment Start Time")]
        //public DateTime StartTime { get; set; }

        //[Required(ErrorMessage = "Please enter the appointment end time.")]
        //[DataType(DataType.DateTime)]
        //[Display(Name = "Appointment End Time")]
        //public DateTime EndTime { get; set; }

        // Navigation property to represent the relationship with the Doctor model
        // Foreign key to link with Patient
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        // Foreign key to link with Doctor
        public int DoctorId { get; set; }
        public Doctor? AssociatedDoctor { get; set; }

        // Additional string property for any extra information
        public string AdditionalInfo { get; set; } = string.Empty;
    }
  
}
