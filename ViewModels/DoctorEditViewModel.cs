namespace PureCareHub_HospitalCare.ViewModels
{
    public class DoctorEditViewModel :DoctorRegistrationViewModel
    {
        public int Id { get; set; } 
        public string ExistingPhotoPath { get; set; } = string.Empty;
    }
}
