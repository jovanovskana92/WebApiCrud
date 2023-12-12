using Microsoft.AspNetCore.Mvc;
using WebApiCrud.Models;
using WebApiCrud.Services;

namespace WebApiCrud.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        // GET: api/Countries
        [HttpGet]
        public ActionResult<IEnumerable<Country>> GetCountries()
        {
            var countries = _countryService.GetCountries();
            return Ok(countries);
        }

        // GET: api/Country/1
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Country))]
        public ActionResult<Country> GetCountry(int id)
        {
            var country = _countryService.GetCountryById(id);

            if (country == null)
            {
                return NotFound($"Country with ID {id} does not exist.");
            }

            return Ok(country);
        }

        // POST: api/Country
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Country))]
        public ActionResult<int> CreateCountry([FromBody] CreateCountryModel createCountryModel)
        {
            try
            {
                var countryId = _countryService.CreateCountry(createCountryModel);
                var createdCountry = _countryService.GetCountryById(countryId);

                return CreatedAtAction(nameof(GetCountry), new { id = countryId }, createdCountry);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Return a 400 Bad Request with the exception message
            }
        }

        // PUT: api/Country/1
        [HttpPut("{id}")]
        public ActionResult<Country> UpdateCountry(int id, [FromBody] UpdateCountryModel updateCountryModel)
        {
            try
            {
                if (updateCountryModel == null)
                {
                    return BadRequest("UpdateCountryModel is required.");
                }

                var updatedCountry = _countryService.UpdateCountry(id, updateCountryModel);
                return Ok(updatedCountry);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // Return a 404 Not Found with the exception message
            }
        }

        // DELETE: api/Country/1
        [HttpDelete("{id}")]
        public IActionResult DeleteCountry(int id)
        {
            var existingCountry = _countryService.GetCountryById(id);

            if (existingCountry == null)
            {
                return NotFound($"Country with ID {id} does not exist or was previously deleted.");
            }

            _countryService.DeleteCountry(id);

            return NoContent();
        }
    }
}
