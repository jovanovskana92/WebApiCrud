using Microsoft.AspNetCore.Mvc;
using WebApiCrud.Models;
using WebApiCrud.Services;

namespace WebApiCrud.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        // GET: api/Companies
        [HttpGet]
        public ActionResult<IEnumerable<Company>> GetCompanies()
        {
            var companies = _companyService.GetCompanies();
            return Ok(companies);
        }

        // GET: api/Company/1
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Company))]
        public ActionResult<Company> GetCompany(int id)
        {
            var company = _companyService.GetCompanyById(id);

            if (company == null)
            {
                return NotFound($"Company with ID {id} does not exist.");
            }

            return Ok(company);
        }

        // POST: api/Company
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Company))]
        public ActionResult<int> CreateCompany([FromBody] CreateCompanyModel createCompanyModel)
        {
            try
            {
                var companyId = _companyService.CreateCompany(createCompanyModel);
                var createdCompany = _companyService.GetCompanyById(companyId);

                return CreatedAtAction(nameof(GetCompany), new { id = companyId }, createdCompany);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Return a 400 Bad Request with the exception message
            }
        }

        // PUT: api/Company/1
        [HttpPut("{id}")]
        public ActionResult<Company> UpdateCompany(int id, [FromBody] UpdateCompanyModel updateCompanyModel)
        {
            try
            {
                if (updateCompanyModel == null)
                {
                    return BadRequest("UpdateCompanyModel is required.");
                }

                var updatedCompany = _companyService.UpdateCompany(id, updateCompanyModel);
                return Ok(updatedCompany);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // Return a 404 Not Found with the exception message
            }
        }

        // DELETE: api/Company/1
        [HttpDelete("{id}")]
        public IActionResult DeleteCompany(int id)
        {
            var existingCompany = _companyService.GetCompanyById(id);

            if (existingCompany == null)
            {
                return NotFound($"Company with ID {id} does not exist or was previously deleted.");
            }

            _companyService.DeleteCompany(id);

            return NoContent();
        }
    }
}
