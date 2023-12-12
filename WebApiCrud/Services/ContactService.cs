using Microsoft.EntityFrameworkCore;
using WebApiCrud.Models;

namespace WebApiCrud.Services
{
    public class ContactService : IContactService
    {
        private readonly ApiContext _dbContext;

        public ContactService(ApiContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int CreateContact(Contact contact)
        {
            _dbContext.Contacts.Add(contact);
            _dbContext.SaveChanges();
            return contact.ContactId;
        }

        public CreateContactResponse UpdateContact(int id, UpdateContactModel updateContactModel)
        {
            var existingContact = _dbContext.Contacts.Find(id);

            if (existingContact == null)
            {
                throw new InvalidOperationException("Contact not found.");
            }
            existingContact.ContactName = updateContactModel.ContactName;
            if (_dbContext.Companies.Find(updateContactModel.CompanyId) == null || _dbContext.Countries.Find(updateContactModel.CountryId) == null)
            {
                throw new InvalidOperationException("Invalid CompanyId or CountryId provided.");
            }

            existingContact.CompanyId = updateContactModel.CompanyId;
            existingContact.CountryId = updateContactModel.CountryId;

            _dbContext.SaveChanges();

            var response = new CreateContactResponse
            {
                ContactId = existingContact.ContactId,
                ContactName = existingContact.ContactName,
                CompanyId = existingContact.CompanyId,
                CountryId = existingContact.CountryId
            };

            return response;
        }

        public void DeleteContact(int id)
        {
            var contact = _dbContext.Contacts.Find(id);

            if (contact != null)
            {
                _dbContext.Contacts.Remove(contact);
                _dbContext.SaveChanges();
            }
        }

        public List<Contact> GetContacts()
        {
            return _dbContext.Contacts
                .Include(c => c.Company)
                .Include(c => c.Country)
                .ToList();
        }

        public List<FilteredContactResponse> FilterContacts(int? countryId, int? companyId)
        {
            var query = _dbContext.Contacts.AsQueryable();

            // Filter by countryId if provided
            if (countryId.HasValue)
            {
                query = query.Where(c => c.CountryId == countryId);
            }

            // Filter by companyId if provided
            if (companyId.HasValue)
            {
                query = query.Where(c => c.CompanyId == companyId);
            }

            var result = query.Select(c => new FilteredContactResponse
            {
                ContactId = c.ContactId,
                ContactName = c.ContactName,
                Company = new CompanyDto
                {
                    CompanyId = c.CompanyId,
                    CompanyName = c.Company.CompanyName
                },
                Country = new CountryDto
                {
                    CountryId = c.CountryId,
                    CountryName = c.Country.CountryName
                }
            }).ToList();

            return result;
        }

        public Contact GetContactWithCompanyAndCountry(int id)
        {
            var contact = _dbContext.Contacts
                .Include(c => c.Company)
                .Include(c => c.Country)
                .FirstOrDefault(c => c.ContactId == id);

            return contact;
        }
    }
}