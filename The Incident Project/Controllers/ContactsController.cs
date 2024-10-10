using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using The_Incident_Project.Models;
using The_Incident_Project.Services;

namespace The_Incident_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        // GET: api/Contacts
        [HttpGet]
        public ActionResult<IEnumerable<Contact>> GetContacts()
        {
            var contacts = _contactService.GetContacts();

            if (contacts == null || !contacts.Any())
            {
                return NotFound();
            }

            return Ok(contacts);
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public ActionResult<Contact> GetContact(int id)
        {
            var contact = _contactService.GetContact(id);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        // PUT: api/Contacts/5
        [HttpPut("{id}")]
        public ActionResult<Contact> PutContact(int id, Contact contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }

            var existingContact = _contactService.GetContactByEmail(contact.Email);
            if (existingContact == null)
            {
                return NotFound();
            }

            return Ok(_contactService.PutContact(contact)); // Return the updated contact or handle the response accordingly
        }

        // POST: api/Contacts
        [HttpPost]
        public ActionResult<Contact> PostContact(Contact contact)
        {
            if (_contactService.GetContactByEmail(contact.Email) != null)
            {
                return BadRequest("This email already exists");
            }
            _contactService.PostContact(contact);

            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            try
            {
                _contactService.DeleteContact(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("No contact with such id");
            }
        }
    }
}
