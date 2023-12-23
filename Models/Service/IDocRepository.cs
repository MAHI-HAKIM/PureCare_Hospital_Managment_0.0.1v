namespace PureCareHub_HospitalCare.Models.Service
{
    public interface IDocRepository
    {
        //Creates a Doctor
        Doctor Add(Doctor doctor);

        // Get a doctor by ID
        Doctor GetDoctorById(int doctorId);

        // Get all doctors
        IEnumerable<Doctor> GetAllDoctors();

        // Update an existing doctor
        Doctor Update(Doctor doctor);

        // Delete a doctor by ID
        Doctor Delete(int doctorId);

        string GetDepartmentName(int departmentId);
    }
}
