namespace WebApiCrud.Models
{
    public class CreateContactDto
    {
        public string ContactName { get; set; }
        public int CompanyId { get; set; }
        public int CountryId { get; set; }
    }
}
