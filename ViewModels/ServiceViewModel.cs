using PureCareHub_HospitalCare.Models;

namespace PureCareHub_HospitalCare.ViewModels
{
    public class ServiceViewModel
    {
        public string ?DepartmentName {  get; set; }
        public string ?DepartmentDescription { get; set; }
        public string? PhotoPath { get; set; }
        public List<Doctor> ? doctorsList { get; set; }
    }
}
