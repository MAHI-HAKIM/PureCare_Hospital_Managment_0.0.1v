using Microsoft.AspNetCore.Identity;
using PureCareHub_HospitalCare.Areas.Identity.Data;
using PureCareHub_HospitalCare.Data;
using PureCareHub_HospitalCare.Models;

namespace PureCareHub_HospitalCare.ViewModels
{
	public class PatientsListViewModel
	{
		public List<Patient> Patients { get; set; }
		public int Doctorscount {  get; set; }
		public int DepartmentsCount {  get; set; }
		public int FullAppointmentCount {  get; set; }
		public int patientCount {  get; set; }
		public Dictionary<string, string>? PatientsEmails { get; set; } // Dictionary to store UserId -> Email mapping
		public Dictionary<string, int> ?AppointmentCounts { get; set; } // Dictionary to store UserId -> Appointment Count mapping


		public async Task PopulateEmailsAndCountsAsync(UserManager<ApplicationUser> userManager, ApplicationDBContext dbContext)
		{
			PatientsEmails = new Dictionary<string, string>();
			AppointmentCounts = new Dictionary<string, int>();

			foreach (var patient in Patients)
			{
				var user = await userManager.FindByIdAsync(patient.UserId);

				if (user != null)
				{
					PatientsEmails.Add(patient.UserId, user.Email);

					// Count the appointments for each patient
					var appointmentCount = dbContext.appointments.Count(a => a.PatientId == patient.Id);
					AppointmentCounts.Add(patient.UserId, appointmentCount);
				}
			}
		}
	}
}
