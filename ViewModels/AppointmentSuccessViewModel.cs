using PureCareHub_HospitalCare.Models;

namespace PureCareHub_HospitalCare.ViewModels
{
    public class AppointmentSuccessViewModel:AppointmentRegistationViewModel
    {
        public Doctor?assignedDoctor {  get; set; }
    }
}
