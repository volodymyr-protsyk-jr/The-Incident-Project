using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using The_Incident_Project.Models;

namespace The_Incident_Project.Services
{
    public interface IAccountService
    {
        public IEnumerable<Account> GetAccounts();
        public Account GetAccount(int id);
        public Account GetAccountByEmail(string email);
        public Account PutAccount(int id, Account account);
        public Account PostAccount(Account account);
        public void DeleteAccount(int id);
        public Account GetAccountByName(string name);

    }
    public class AccountService(IncidentContext context) : IAccountService
    {
        private ContactService contactService = new ContactService(context);
        public void DeleteAccount(int id)
        {
            Account accToDelete = GetAccount(id);
            if (accToDelete == null)
            {
                throw new KeyNotFoundException("No such account");
            }
            context.Accounts.Remove(GetAccount(id));
            context.SaveChanges();

        }

        public Account GetAccount(int id)
        {
            Account acc = context.Accounts.Include(a => a.Contacts).SingleOrDefault(x => x.Id == id);
            if (acc == null)
            {
                throw new KeyNotFoundException("No such account");
            }
            return acc;
        }

        public Account GetAccountByEmail(string email)
        {
            return context.Accounts.SingleOrDefault(a => a.Contacts.Any(x => x.Email == email));
        }

        public IEnumerable<Account> GetAccounts()
        {
            return context.Accounts.Include(x => x.Contacts);
        }

        public Account PostAccount(Account account)
        {
            List<Contact> contacts = new List<Contact>();
            foreach (Contact contact in account.Contacts)
            {
                if (contactService.GetContact(contact.Id) != null)
                {
                    contacts.Add(contactService.PutContact(contact));
                }
                else
                {
                    contacts.Add(contactService.PostContact(contact));
                }
            }
            Account newAccount = new Account() { Contacts = contacts, Name = account.Name, Id = account.Id };
            context.Accounts.Add(account);
            context.SaveChanges();
            return GetAccountByName(newAccount.Name);
        }

        public Account PutAccount(int id, Account account)
        {
            Account acc = GetAccount(id);
            acc.Name = account.Name;
            List<Contact> updatedContacts = new();
            foreach (Contact contact in account.Contacts)
            {
                updatedContacts.Add(contactService.PutContact(contact));
            }
            acc.Contacts = updatedContacts;

            context.SaveChanges();
            return GetAccountByName(account.Name);
        }

        public Account GetAccountByName(string name)
        {
            return context.Accounts.SingleOrDefault(x => x.Name == name);
        }
    }
}
