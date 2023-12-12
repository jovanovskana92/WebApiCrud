using WebApiCrud.Controllers;
using WebApiCrud.Models;

namespace WebApiCrud.Services
{
    public class CompanyService : ICompanyService

    {
        public bool CompanyExists(int companyId)
        {
            // Check if a company with the specified ID exists
            return _dbContext.Companies.Any(c => c.CompanyId == companyId);
        }
        private readonly ApiContext _dbContext;

        public CompanyService(ApiContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Company> GetCompanies()
        {
            return _dbContext.Companies.ToList();
        }

        public Company GetCompanyById(int companyId)
        {
            return _dbContext.Companies.FirstOrDefault(c => c.CompanyId == companyId);
        }

        public int CreateCompany(Company company)
        {
            _dbContext.Companies.Add(company);
            _dbContext.SaveChanges();
            return company.CompanyId;
        }

        public Company UpdateCompany(int companyId, UpdateCompanyModel updateCompanyModel)
        {
            // Check if the company with the specified ID exists
            var existingCompany = _dbContext.Companies.Find(companyId);
            if (existingCompany == null)
            {
                throw new InvalidOperationException($"Company with ID {companyId} does not exist.");
            }
            var duplicateCompany = _dbContext.Companies.FirstOrDefault(c => c.CompanyName == updateCompanyModel.CompanyName && c.CompanyId != companyId);
            if (duplicateCompany != null)
            {
                throw new InvalidOperationException($"Company with name {updateCompanyModel.CompanyName} already exists.");
            }
            if (!string.IsNullOrEmpty(updateCompanyModel.CompanyName))
            {
                existingCompany.CompanyName = updateCompanyModel.CompanyName;
            }

            _dbContext.SaveChanges();

            return existingCompany;
        }

        public void DeleteCompany(int companyId)
        {
            var company = _dbContext.Companies.Find(companyId);

            if (company != null)
            {
                _dbContext.Companies.Remove(company);
                _dbContext.SaveChanges();
            }
        }
        public int CreateCompany(CreateCompanyModel createCompanyModel)
        {
            if (createCompanyModel == null)
            {
                throw new ArgumentNullException(nameof(createCompanyModel));
            }
            // Check if a company with the same name already exists
            if (_dbContext.Companies.Any(c => c.CompanyName == createCompanyModel.CompanyName))
            {
                throw new InvalidOperationException("A company with the same name already exists.");
            }
            var newCompany = new Company
            {
                CompanyName = createCompanyModel.CompanyName
            };

            _dbContext.Companies.Add(newCompany);

            _dbContext.SaveChanges();

            return newCompany.CompanyId;
        }
    }
}
