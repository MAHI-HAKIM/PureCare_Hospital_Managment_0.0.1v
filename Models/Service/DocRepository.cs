using Microsoft.EntityFrameworkCore;
using PureCareHub_HospitalCare.Data;

namespace PureCareHub_HospitalCare.Models.Service
{
    public class DocRepository : IDocRepository
    {

        private readonly ApplicationDBContext context;
        public DocRepository(ApplicationDBContext _context)
        {
            context = _context;
        }

        Doctor IDocRepository.Add(Doctor doctor)
        {
           context.doctors.Add(doctor);
            context.SaveChanges();
            return doctor;
        }

        Doctor IDocRepository.Delete(int doctorId)
        {
            Doctor doctor = context.doctors.Find(doctorId);
            if(doctor != null)
            {
                context.doctors.Remove(doctor);
                context.SaveChanges();
            }
            return doctor;
        }

        IEnumerable<Doctor> IDocRepository.GetAllDoctors()
        {
            return context.doctors;
        }

        Doctor IDocRepository.GetDoctorById(int doctorId)
        {
            return context.doctors.Find(doctorId);
        }

        Doctor IDocRepository.Update(Doctor doctorChange)
        {
            var doctor = context.doctors.Attach(doctorChange);

            doctor.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return doctorChange;
        }
      
    }
}
