using Model;

namespace RepositoryContract
{
    public interface ICountriesRepository
    {
        Task<Country> AddCountry(Country country);
        Task<Country?> GetCountryById(Guid CountryId);
        Task<Country?> GetCountryByName(string countryName);
        Task<List<Country>> GetAll();
    }
}
