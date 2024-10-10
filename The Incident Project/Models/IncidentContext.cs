using Microsoft.EntityFrameworkCore;
using The_Incident_Project.Models;
using The_Incident_Project.Services;

namespace The_Incident_Project.Models
{
    public class IncidentContext(DbContextOptions<IncidentContext> options) : DbContext(options)
    {
        public DbSet<The_Incident_Project.Models.Account> Accounts { get; set; }
        public DbSet<The_Incident_Project.Models.Contact> Contacts { get; set; }
        public DbSet<The_Incident_Project.Models.Incident> Incidents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Name)
                .IsUnique();
            
            // Ensure IncidentName is unique
            modelBuilder.Entity<Incident>()
                .HasIndex(i => i.IncidentName)
                .IsUnique();
        }

    }
}

