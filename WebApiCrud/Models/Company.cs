using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace WebApiCrud.Models
{
    public class Company
    {
        [Key]
        [BindNever]
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}
