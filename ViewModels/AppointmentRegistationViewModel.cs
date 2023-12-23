using Microsoft.AspNetCore.Mvc.Rendering;
using PureCareHub_HospitalCare.Models;
using System.ComponentModel.DataAnnotations;

namespace PureCareHub_HospitalCare.ViewModels
{
    public class AppointmentRegistationViewModel
    {

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
        public int PatientId { get; set; }


        // Foreign key to link with Doctor
        [Required(ErrorMessage = "Please select a doctor.")]
        [Display(Name = "Select Doctor")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Please select a doctor.")]
        public  Doctor? Doctors { get; set; }

        [Required(ErrorMessage = "Please Department a doctor.")]
        public int SelectedDepartmentId { get; set; }

        [Required(ErrorMessage = "Please select a department.")]
        public SelectList? DepartmentsList { get; set; }

        public Doctor ?assingedDoctor { get; set; }

        // Additional string property for any extra information
        public string AdditionalInfo { get; set; } = string.Empty;
    }
}
