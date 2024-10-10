using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using The_Incident_Project.Models;

namespace The_Incident_Project.Services
{
    public interface IIncidentService
    {
        IEnumerable<Incident> GetIncidents();
        Incident GetIncident(string incidentName);
        Incident PostIncident(Incident incident);
        Incident PutIncident(string incidentName, Incident incident);
        void DeleteIncident(string incidentName);
    }

    public class IncidentService(IncidentContext context) : IIncidentService
    {
        private ContactService contactService = new(context);
        private AccountService accountService = new(context);
        private readonly IncidentContext _context = context;



        public void DeleteIncident(string incidentName)
        {
            Incident incidentToDelete = GetIncident(incidentName);
            if (incidentToDelete == null)
            {
                throw new KeyNotFoundException("No such incident");
            }
            _context.Incidents.Remove(incidentToDelete);
            _context.SaveChanges();
        }

        public Incident GetIncident(string incidentName)
        {
            Incident incident = _context.Incidents.Include(i => i.Accounts).SingleOrDefault(x => x.IncidentName == incidentName);
            if (incident == null)
            {
                throw new KeyNotFoundException("No such incident");
            }
            return incident;
        }

        public IEnumerable<Incident> GetIncidents()
        {
            return _context.Incidents.Include(a => a.Accounts).ThenInclude(c => c.Contacts);
        }

        public Incident PostIncident(Incident incident)
        {
            foreach (Account account in incident.Accounts)
            {
                if (accountService.GetAccountByName(account.Name) != null)
                {
                    Account updAcc = accountService.GetAccountByName(account.Name);
                    accountService.PutAccount(updAcc.Id, updAcc);
                }
                else
                {
                    throw new KeyNotFoundException("No such account in system");
                }
            }

            Incident newInc = new Incident() { Accounts = incident.Accounts, IncidentName = incident.IncidentName, Description = incident.Description };


            _context.Incidents.Add(newInc);
            return GetIncident(incident.IncidentName);
        }

        public Incident PutIncident(string incidentName, Incident incident)
        {
            Incident existingIncident = GetIncident(incidentName);
            if (existingIncident == null)
            {
                throw new KeyNotFoundException();
            }
            existingIncident.Description = incident.Description;
            foreach (Account account in incident.Accounts)
            {
                Account accToUpdate = accountService.GetAccount(account.Id);
                if (accToUpdate == null)
                {
                    throw new KeyNotFoundException("No such account in system");
                }
                if (existingIncident.Accounts.Any(a => a.Name == accToUpdate.Name))
                {

                    accountService.PutAccount(account.Id, account);
                }
                else
                {
                    existingIncident.Accounts.Add(account);
                }
            }
            _context.SaveChanges();
            return existingIncident;
        }
    }
}
