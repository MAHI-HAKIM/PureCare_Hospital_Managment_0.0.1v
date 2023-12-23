using PureCareHub_HospitalCare.Models;

namespace PureCareHub_HospitalCare.ViewModels
{
    public class DoctorViewModel
    {
        public Doctor? doctor { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }
}
