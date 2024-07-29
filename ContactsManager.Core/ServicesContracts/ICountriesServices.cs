using Microsoft.AspNetCore.Http;
using ServiceContract.DTO;

namespace ServiceContract
{
    public interface ICountriesServices
    {
        Task<CountryResponse> AddCountry(CountryAddRequest? countryRequest);
        Task<List<CountryResponse>> GetAllCountry();
        Task<CountryResponse?> GetCountryBy(Guid? countryId);
        /// <summary>
        /// Upload Countries from excel file into database
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns>Return Number of countries added</returns>
        Task<int>UploadCountriesFromExcelFile(IFormFile formFile);
    }
}
