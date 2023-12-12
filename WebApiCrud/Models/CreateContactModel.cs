namespace WebApiCrud.Models
{
    public class CreateContactResponse
    {
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
