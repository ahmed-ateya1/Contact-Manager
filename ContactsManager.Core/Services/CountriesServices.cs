using Microsoft.AspNetCore.Http;
using Model;
using OfficeOpenXml;
using RepositoryContract;
using ServiceContract;
using ServiceContract.DTO;

namespace Services
{
    public class CountriesServices : ICountriesServices
    {
        private readonly ICountriesRepository _countriesRepository;

        public CountriesServices(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryRequest)
        {
            if(countryRequest == null) 
                throw new ArgumentNullException(nameof(countryRequest));
            if(countryRequest.CountryName == null)
                throw new ArgumentException(nameof(countryRequest.CountryName));

            if (await _countriesRepository.
                GetCountryByName(countryRequest.CountryName) != null)
                throw new ArgumentException("Country Name can't be duplicated");

            var country = countryRequest.ToCountry();
            country.CountryId = Guid.NewGuid();

            await _countriesRepository.AddCountry(country);

            return country.ToCountry();
        }

        public async Task<List<CountryResponse>> GetAllCountry()
        {
            List<Country> countries = await _countriesRepository.GetAll();
            return countries
                .Select(x=>x.ToCountry())
                .ToList();
        }

        public async Task<CountryResponse?> GetCountryBy(Guid? countryId)
        {
            if(countryId == null)   
                return null;
            var country = await _countriesRepository.
                GetCountryById(countryId.Value);

            if(country == null)
                return null;
            return country.ToCountry();
        }

        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            int countriesInserted = 0;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets["Countries"];

                int rowCount = workSheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string? cellValue = Convert.ToString(workSheet.Cells[row, 1].Value);

                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string? countryName = cellValue;

                        if (await _countriesRepository.GetCountryByName(countryName) == null)
                        {
                            Country country = new Country() {
                                CountryId = Guid.NewGuid(), 
                                CountryName = countryName 
                            };
                            await _countriesRepository.AddCountry(country);

                            countriesInserted++;
                        }
                    }
                }
            }

            return countriesInserted;
        }
    }
}
