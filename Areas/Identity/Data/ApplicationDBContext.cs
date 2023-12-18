using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PureCare.Doctors.Model;
using PureCareHub_HospitalCare.Areas.Identity.Data;
using PureCareHub_HospitalCare.Models;

namespace PureCareHub_HospitalCare.Data;

public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Patient>? Patients { get; set; }
    public DbSet<Doctor> ? Doctors{ get;set;}

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
    }
}
