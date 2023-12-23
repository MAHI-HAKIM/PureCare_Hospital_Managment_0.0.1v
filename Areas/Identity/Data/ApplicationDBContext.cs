using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PureCareHub_HospitalCare.Areas.Identity.Data;
using PureCareHub_HospitalCare.Models;
using System.Reflection.Emit;

namespace PureCareHub_HospitalCare.Data;

public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Patient>? patients { get; set; }
    public DbSet<Doctor>? doctors { get; set; }
    public DbSet<Appointment>? appointments { get; set; }
    public DbSet<Department>? Departments { get; set; }


    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
         builder.Entity<Patient>()
            .HasOne(p => p.User)
            .WithMany(u => u.Patients)
            .HasForeignKey(p => p.UserId)
            .IsRequired();

        builder.Entity<Appointment>()
        .HasOne(p => p.Patient)
        .WithMany(a => a.Appointments)
        .HasForeignKey(p => p.PatientId)
        .IsRequired();

        builder.Entity<Appointment>()
       .HasOne(d => d.AssociatedDoctor)
       .WithMany(a => a.Appointments)
       .HasForeignKey(p => p.DoctorId)
       .IsRequired();

        builder.Entity<Doctor>()
            .HasOne(d => d.department)
            .WithMany()
            .HasForeignKey(d => d.depID)
            .IsRequired();

    }
}
