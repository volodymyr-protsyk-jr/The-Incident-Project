using System.ComponentModel.DataAnnotations;

namespace The_Incident_Project.Models
{
    public class Account
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public required ICollection<Contact> Contacts { get; set; }

        public Account ()
        {
            Contacts = new List<Contact>();
        }
    }
}
