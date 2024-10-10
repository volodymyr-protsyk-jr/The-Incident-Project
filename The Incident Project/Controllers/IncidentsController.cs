using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using The_Incident_Project.Models;
using The_Incident_Project.Services;

namespace The_Incident_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        private readonly IIncidentService _incidentService;
        private readonly IAccountService _accountService;
        private readonly IContactService _contactService;

        public IncidentsController(IAccountService accountService, IContactService contactService, IIncidentService incidentService)
        {
            _accountService = accountService;
            _contactService = contactService;
            _incidentService = incidentService;
        }


        // GET: api/Incidents
        [HttpGet]
        public ActionResult<IEnumerable<Incident>> GetIncidents()
        {
            var incidents = _incidentService.GetIncidents();
            return Ok(incidents);
        }

        // GET: api/Incidents/5
        [HttpGet("{incidentName}")]
        public ActionResult<Incident> GetIncident(string incidentName)
        {
            try
            {
                var incident = _incidentService.GetIncident(incidentName);
                return Ok(incident);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // PUT: api/Incidents/5
        [HttpPut("{id}")]
        public IActionResult PutIncident(string incidentName, Incident incident)
        {
            if (incidentName != incident.IncidentName)
            {
                return BadRequest();
            }
            if (incident.Accounts == null || !incident.Accounts.Any())
            {
                return BadRequest("At least one account must be provided.");
            }

            try
            {
                _incidentService.PutIncident(incidentName, incident);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Incidents
        [HttpPost]
        public ActionResult<Incident> PostIncident(string accountName,
                                                    string contactFirstName,
                                                    string contactLastName,
                                                    string contactEmail,
                                                    string incidentDescription)
        {
            if (string.IsNullOrEmpty(accountName) || string.IsNullOrEmpty(contactFirstName) ||
                string.IsNullOrEmpty(contactLastName) || string.IsNullOrEmpty(contactEmail) ||
                string.IsNullOrEmpty(incidentDescription))
            {
                return BadRequest("All parameters must be provided.");
            }

            var incident = new Incident
            {
                Description = incidentDescription,
                Accounts = new List<Account>() // Create a new list for accounts if needed
                // Populate accounts based on your logic (this part depends on your application needs)
            };

            // Create and return the new incident
            var createdIncident = _incidentService.PostIncident(incident);
            return CreatedAtAction(nameof(GetIncident), new { id = createdIncident.IncidentName }, createdIncident);
        }

        // DELETE: api/Incidents/5
        [HttpDelete("{id}")]
        public IActionResult DeleteIncident(string incidentName)
        {
            try
            {
                _incidentService.DeleteIncident(incidentName);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
