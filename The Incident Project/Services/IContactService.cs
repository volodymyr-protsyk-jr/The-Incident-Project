using Microsoft.AspNetCore.Mvc;
using The_Incident_Project.Models;

namespace The_Incident_Project.Services
{
    public interface IContactService
    {
        public IEnumerable<Contact> GetContacts();
        public Contact GetContact(int id);
        public Contact PutContact(Contact contact);
        public Contact PostContact(Contact contact);
        public void DeleteContact(int id);

        public Contact GetContactByEmail(string email);

    }

    public class ContactService(IncidentContext context) : IContactService
    {

        public void DeleteContact(int id)
        {
            Contact contactToDelete = context.Contacts.Find(id);
            if (contactToDelete==null)
            {
                throw new KeyNotFoundException();
            }
            context.Contacts.Remove(contactToDelete);
            context.SaveChanges();
        }

        public Contact GetContact(int id)
        {
            return context.Contacts.Find(id);
        }

        public Contact GetContactByEmail(string email)
        {
            return context.Contacts.SingleOrDefault(x => x.Email == email);
        }

        public IEnumerable<Contact> GetContacts()
        {
            return context.Contacts.ToList();
        }

        public Contact PostContact(Contact contact)
        {
            context.Contacts.Add(contact);
            context.SaveChanges();
            return contact;
        }

        public Contact PutContact(Contact contact)
        {
            var existingContact = GetContactByEmail(contact.Email);
            existingContact.FirstName = contact.FirstName;
            existingContact.LastName = contact.LastName;
            existingContact.Email = contact.Email;
            context.SaveChanges();
            return GetContactByEmail(contact.Email);
        }

    }
}
