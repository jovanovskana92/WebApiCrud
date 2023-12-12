namespace WebApiCrud.Models
{
    public class ContactDto
    {
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
    public class FilteredContactResponse
    {
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public CompanyDto Company { get; set; }
        public CountryDto Country { get; set; }
    }

    public class CompanyDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }

    public class CountryDto
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }

}
