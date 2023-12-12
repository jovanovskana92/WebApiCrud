using Microsoft.EntityFrameworkCore;

namespace WebApiCrud.Models
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .Property(c => c.CompanyId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Country>()
                .Property(c => c.CountryId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Contact>()
                .Property(c => c.ContactId)
                .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}
