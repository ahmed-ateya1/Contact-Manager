using ContactsManager.Core.Domain.IdentityEntites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using System.Text.Json;

namespace Model.AppDbContext
{
    public class PersonDbContext : IdentityDbContext<ApplicationUser , ApplicationRole , Guid>
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Country> Countries { get; set; }

        public PersonDbContext() { }

        public PersonDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           modelBuilder.ApplyConfigurationsFromAssembly(typeof(CountryConfiguration).Assembly);

            try
            {
                var countriesJson = File.ReadAllText("Countries.json");
                List<Country> countries = JsonSerializer.Deserialize<List<Country>>(countriesJson);

                foreach (var country in countries)
                {
                    modelBuilder.Entity<Country>().HasData(country);
                }

                var personsJson = File.ReadAllText("Persons.json");
                List<Person> persons = JsonSerializer.Deserialize<List<Person>>(personsJson);

                foreach (var person in persons)
                {
                    modelBuilder.Entity<Person>().HasData(person);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"An error occurred while reading seed data: {ex.Message}");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public List<Person> sp_GetAllPerson()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GETALLPERSON]").ToList();
        }
    }
}