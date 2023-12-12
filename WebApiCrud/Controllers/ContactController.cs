using Microsoft.AspNetCore.Mvc;
using WebApiCrud.Models;
using WebApiCrud.Services;

namespace WebApiCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly ICompanyService _companyService;
        private readonly ICountryService _countryService;

        public ContactController(IContactService contactService, ICompanyService companyService, ICountryService countryService)
        {
            _contactService = contactService;
            _companyService = companyService;
            _countryService = countryService;
        }

        // POST: api/Contacts
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateContactResponse))]
        public ActionResult<int> CreateContact([FromBody] CreateContactDto createContactDto)
        {
            var errorMessages = new List<string>();

            // Check if the company exists
            var companyExists = _companyService.CompanyExists(createContactDto.CompanyId);
            if (!companyExists)
            {
                errorMessages.Add($"Company with id {createContactDto.CompanyId} does not exist.");
            }
            // Check if the country exists
            var countryExists = _countryService.CountryExists(createContactDto.CountryId);
            if (!countryExists)
            {
                errorMessages.Add($"Country with id {createContactDto.CountryId} does not exist.");
            }

            if (errorMessages.Any())
            {
                return BadRequest(errorMessages);
            }

            var contact = new Contact
            {
                ContactName = createContactDto.ContactName,
                CompanyId = createContactDto.CompanyId,
                CountryId = createContactDto.CountryId
            };

            var contactId = _contactService.CreateContact(contact);
            var company = _companyService.GetCompanyById(contact.CompanyId);
            var country = _countryService.GetCountryById(contact.CountryId);
            var response = new CreateContactResponse
            {
                ContactId = contactId,
                ContactName = contact.ContactName,
                CompanyId = contact.CompanyId,
                CompanyName = company?.CompanyName,
                CountryId = contact.CountryId,
                CountryName = country?.CountryName
            };

            return CreatedAtAction(nameof(GetContact), new { id = contactId }, response);
        }

        // PUT: api/Contacts/1
        [HttpPut("{id}")]
        public ActionResult<CreateContactResponse> UpdateContact(int id, [FromBody] UpdateContactModel updateContactModel)
        {
            try
            {
                if (updateContactModel == null)
                {
                    return BadRequest("UpdateContactModel is required.");
                }

                var updatedContact = _contactService.UpdateContact(id, updateContactModel);

                // Fetch Company and Country
                var company = _companyService.GetCompanyById(updatedContact.CompanyId);
                var country = _countryService.GetCountryById(updatedContact.CountryId);

                // Create response
                var response = new CreateContactResponse
                {
                    ContactId = updatedContact.ContactId,
                    ContactName = updatedContact.ContactName,
                    CompanyId = updatedContact.CompanyId,
                    CompanyName = company?.CompanyName,
                    CountryId = updatedContact.CountryId,
                    CountryName = country?.CountryName
                };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // Return a 404 Not Found with the exception message
            }
        }

        // DELETE: api/Contacts/1
        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            _contactService.DeleteContact(id);
            return NoContent();
        }

        // GET: api/Contacts
        [HttpGet]
        public ActionResult<IEnumerable<CreateContactResponse>> GetContacts()
        {
            var contacts = _contactService.GetContacts();
            var response = contacts.Select(contact => new CreateContactResponse
            {
                ContactId = contact.ContactId,
                ContactName = contact.ContactName,
                CompanyId = contact.CompanyId,
                CompanyName = contact.Company?.CompanyName,
                CountryId = contact.CountryId,
                CountryName = contact.Country?.CountryName
            }).ToList();

            return Ok(response);
        }

        // GET: api/Contacts/1
        [HttpGet("{id}")]
        public ActionResult<object> GetContact(int id)
        {
            var contact = _contactService.GetContactWithCompanyAndCountry(id);

            if (contact == null)
            {
                return NotFound();
            }

            // Create the response with anonymous object
            var response = new
            {
                ContactId = contact.ContactId,
                ContactName = contact.ContactName,
                Company = new
                {
                    CompanyId = contact.Company.CompanyId,
                    CompanyName = contact.Company.CompanyName
                },
                Country = new
                {
                    CountryId = contact.Country.CountryId,
                    CountryName = contact.Country.CountryName
                }
            };

            return Ok(response);
        }

        // GET: api/Contacts/Filter
        [HttpGet("Filter")]
        public ActionResult<IEnumerable<FilteredContactResponse>> FilterContacts([FromQuery] int? countryId, [FromQuery] int? companyId)
        {
            var filteredContacts = _contactService.FilterContacts(countryId, companyId);

            if (filteredContacts == null || !filteredContacts.Any())
            {
                return NotFound("No contacts found matching the specified criteria.");
            }

            return Ok(filteredContacts);
        }

    }
}

