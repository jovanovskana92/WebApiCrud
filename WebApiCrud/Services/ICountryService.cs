using WebApiCrud.Controllers;
using WebApiCrud.Models;

namespace WebApiCrud.Services
{
    public interface ICountryService
    {
        List<Country> GetCountries();
        Country GetCountryById(int countryId);
        int CreateCountry(CreateCountryModel createCountryModel);
        Country UpdateCountry(int countryId, UpdateCountryModel updateCountryModel);
        void DeleteCountry(int countryId);
        bool CountryExists(int countryId);
    }
}
