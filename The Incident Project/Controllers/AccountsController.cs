using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using The_Incident_Project.Models;
using The_Incident_Project.Services;

namespace The_Incident_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IContactService _contactService;

        public AccountsController(IAccountService accountService, IContactService contactService)
        {
            _accountService = accountService;
            _contactService = contactService;
        }

        // GET: api/Accounts
        [HttpGet]
        public ActionResult<IEnumerable<Account>> GetAccounts()
        {
            var accounts = _accountService.GetAccounts();

            if (accounts == null || !accounts.Any())
            {
                return NotFound();
            }

            return Ok(accounts);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public ActionResult<Account> GetAccount(int id)
        {
            var account = _accountService.GetAccount(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        public ActionResult<Account> PutAccount(int id, Account account)
        {
            if (account == null)
            {
                return BadRequest("No account provided");
            }

            // Validate the account's ID
            if (id != account.Id)
            {
                return BadRequest("Account ID mismatch");
            }

            // Update contacts and account
            List<Contact> newContacts = new List<Contact>();
            foreach (var contact in account.Contacts)
            {
                newContacts.Add(_contactService.PutContact(contact));
            }

            Account updatedAccount = _accountService.PutAccount(id, new Account
            {
                Contacts = newContacts,
                Name = account.Name,
                Id = account.Id
            });

            return Ok(updatedAccount);
        }

        // POST: api/Accounts
        [HttpPost]
        public ActionResult<Account> PostAccount(Account account)
        {
            if (account == null || account.Contacts == null || account.Contacts.Count == 0)
            {
                return BadRequest("No contacts provided");
            }

            if (_accountService.GetAccountByName(account.Name) != null)
            {
                return BadRequest($"{account.Name} is already in the database");
            }

            List<Contact> newContacts = new List<Contact>();
            foreach (var contact in account.Contacts)
            {
                if (_contactService.GetContactByEmail(contact.Email) != null)
                {
                    return BadRequest($"{contact.Email} is already linked to an account");
                }
                newContacts.Add(_contactService.PostContact(contact));
            }

            Account newAcc = new Account { Name = account.Name, Contacts = newContacts };
            _accountService.PostAccount(newAcc);
            return CreatedAtAction(nameof(GetAccount), new { id = newAcc.Id }, newAcc);
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            try
            {
                _accountService.DeleteAccount(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("No account with such ID");
            }
        }
    }
}
