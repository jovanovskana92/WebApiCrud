using WebApiCrud.Models;

namespace WebApiCrud.Services
{
    public interface IContactService
    {
        int CreateContact(Contact contact);
        CreateContactResponse UpdateContact(int id, UpdateContactModel updateContactModel);
        void DeleteContact(int id);
        List<Contact> GetContacts();
        List<FilteredContactResponse> FilterContacts(int? countryId, int? companyId);
        Contact GetContactWithCompanyAndCountry(int id);

    }
}
