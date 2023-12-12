using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCrud.Models
{
    public class Contact
    {
        public int ContactId { get; set; }
        public string ContactName { get; set; }

        [ForeignKey("CompanyId")]
        public int CompanyId { get; set; }
        [ForeignKey("CountryId")]
        public int CountryId { get; set; }
        public Company Company { get; set; }
        public Country Country { get; set; }
    }
}
