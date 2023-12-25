using PureCareHub_HospitalCare.Models;

namespace PureCareHub_HospitalCare.ViewModels
{
    public class MedicalHistoryViewModel
    {
        public Appointment? appointments { get; set; }
        public Doctor? appointedDoctor { get; set; }
        public string ? DepartmentName {  get; set; }
    }
}
