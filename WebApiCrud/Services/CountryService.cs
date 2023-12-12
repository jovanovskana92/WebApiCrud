using Microsoft.EntityFrameworkCore;
using WebApiCrud.Controllers;
using WebApiCrud.Models;

namespace WebApiCrud.Services
{
    public class CountryService : ICountryService
    {
        private readonly ApiContext _dbContext;

        public CountryService(ApiContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Country> GetCountries()
        {
            return _dbContext.Countries.ToList();
        }

        public Country GetCountryById(int countryId)
        {
            return _dbContext.Countries.FirstOrDefault(c => c.CountryId == countryId);
        }

        public int CreateCountry(Country country)
        {
            _dbContext.Countries.Add(country);
            _dbContext.SaveChanges();
            return country.CountryId;
        }

        public Country UpdateCountry(int countryId, UpdateCountryModel updateCountryModel)
        {
            // Check if the country with the specified ID exists
            var existingCountry = _dbContext.Countries.Find(countryId);
            if (existingCountry == null)
            {
                throw new InvalidOperationException($"Country with ID {countryId} does not exist.");
            }
            var duplicateCountry = _dbContext.Countries.FirstOrDefault(c => c.CountryName == updateCountryModel.CountryName && c.CountryId != countryId);
            if (duplicateCountry != null)
            {
                throw new InvalidOperationException($"Country with name {updateCountryModel.CountryName} already exists.");
            }
            if (!string.IsNullOrEmpty(updateCountryModel.CountryName))
            {
                existingCountry.CountryName = updateCountryModel.CountryName;
            }

            _dbContext.SaveChanges();

            return existingCountry;
        }

        public void DeleteCountry(int countryId)
        {
            var country = _dbContext.Countries.Find(countryId);

            if (country != null)
            {
                _dbContext.Countries.Remove(country);
                _dbContext.SaveChanges();
            }
        }

        public int CreateCountry(CreateCountryModel createCountryModel)
        {
            if (createCountryModel == null)
            {
                throw new ArgumentNullException(nameof(createCountryModel));
            }
            // Check if a country with the same name already exists
            if (_dbContext.Countries.Any(c => c.CountryName == createCountryModel.CountryName))
            {
                throw new InvalidOperationException("A country with the same name already exists.");
            }

            // Creating a new country entity
            var newCountry = new Country
            {
                CountryName = createCountryModel.CountryName
            };

            // Adding the new country to the context
            _dbContext.Countries.Add(newCountry);

            // Saving changes to the database
            _dbContext.SaveChanges();

            // Returning the newly created country's ID
            return newCountry.CountryId;
        }
        public bool CountryExists(int countryId)
        {
            // Check if a country with the specified ID exists
            return _dbContext.Countries.Any(c => c.CountryId == countryId);
        }
    }
}
