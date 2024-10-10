using System.ComponentModel.DataAnnotations;

namespace The_Incident_Project.Models
{
    public class Incident
    {
        [Key]
        public string IncidentName { get; set; } = GenerateIncidentName();

        [Required]
        public required string Description { get; set; }

        [Required]
        public required ICollection<Account> Accounts { get; set; } = new List<Account>();

        // Static method to generate a unique incident name
        private static string GenerateIncidentName()
        {
            return Guid.NewGuid().ToString(); // Or any custom logic to generate a unique string
        }
        public Incident()
        {
            Accounts = new List<Account>();
        }
    }
}
