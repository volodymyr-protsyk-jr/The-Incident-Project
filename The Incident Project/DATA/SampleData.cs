using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using The_Incident_Project.Models;

namespace The_Incident_Project.DATA
{
    public class SampleData
    {
        public static void Initialize(IncidentContext context)
        {
            context.Incidents.RemoveRange(context.Incidents.ToList());
            context.Accounts.RemoveRange(context.Accounts.ToList());
            context.Contacts.RemoveRange(context.Contacts.ToList());
            context.SaveChanges();

            if (context.Accounts.Count() + context.Contacts.Count() + context.Incidents.Count() != 0)
            {
                return; // DB has been seeded
            }

            var contact1 = new Contact { FirstName = "John", LastName = "Smith", Email = "john.smith@mail.com" };
            var contact2 = new Contact { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@mail.com" };
            var contact3 = new Contact { FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@mail.com" };
            var contact4 = new Contact { FirstName = "Bob", LastName = "Brown", Email = "bob.brown@mail.com" };
            var contact5 = new Contact { FirstName = "Charlie", LastName = "Davis", Email = "charlie.davis@mail.com" };
            var contact6 = new Contact { FirstName = "Diana", LastName = "Evans", Email = "diana.evans@mail.com" };

            var account1 = new Account
            {
                Name = "Tech Solutions",
                Contacts = new List<Contact> { contact1, contact2 }
            };

            var account2 = new Account
            {
                Name = "Financial Services",
                Contacts = new List<Contact> { contact3, contact4 }
            };

            var account3 = new Account
            {
                Name = "Health Services",
                Contacts = new List<Contact> { contact5, contact6 }
            };

            var incident1 = new Incident
            {
                IncidentName = Guid.NewGuid().ToString(), 
                Description = "The server was down for 2 hours.",
                Accounts = new List<Account> { account1 } 
            };

            var incident2 = new Incident
            {
                IncidentName = Guid.NewGuid().ToString(), 
                Description = "Sensitive data was exposed.",
                Accounts = new List<Account> { account2 } 
            };

            var incident3 = new Incident
            {
                IncidentName = Guid.NewGuid().ToString(),
                Description = "Patient records were lost during transfer.",
                Accounts = new List<Account> { account3 } 
            };

            context.Contacts.AddRange(contact1, contact2, contact3, contact4, contact5, contact6);
            context.Accounts.AddRange(account1, account2, account3);

            context.Incidents.AddRange(incident1, incident2, incident3);

            context.SaveChanges();
        }
    }
}
