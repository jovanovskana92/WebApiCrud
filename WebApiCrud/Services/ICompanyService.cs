using WebApiCrud.Controllers;
using WebApiCrud.Models;

namespace WebApiCrud.Services
{
    public interface ICompanyService
    {
        List<Company> GetCompanies();
        Company GetCompanyById(int companyId);
        int CreateCompany(CreateCompanyModel createCompanyModel);
        Company UpdateCompany(int companyId, UpdateCompanyModel updateCompanyModel);
        void DeleteCompany(int companyId);
        bool CompanyExists(int companyId);
    }
}
