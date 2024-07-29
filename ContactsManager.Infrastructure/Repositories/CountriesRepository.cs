using Microsoft.EntityFrameworkCore;
using Model;
using Model.AppDbContext;
using RepositoryContract;


namespace Repository
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly PersonDbContext _db;

        public CountriesRepository(PersonDbContext db)
        {
            _db = db;
        }

        public async Task<Country> AddCountry(Country country)
        {
            await _db.Countries.AddAsync(country);
            await _db.SaveChangesAsync();
            return country;
        }

        public async Task<List<Country>> GetAll()
        {
           return await _db.Countries.ToListAsync();
        }

        public async Task<Country?> GetCountryById(Guid CountryId)
        {
            return await _db.Countries.
                FirstOrDefaultAsync(x=>x.CountryId == CountryId);
        }

        public async Task<Country?> GetCountryByName(string countryName)
        {
           return await _db.Countries.
                FirstOrDefaultAsync(x=>x.CountryName==countryName);
        }
    }
}
