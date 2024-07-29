using Microsoft.AspNetCore.Http;
using Model;
using Moq;
using RepositoryContract;
using ServiceContract;
using ServiceContract.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ServicesTest
{
    public class CountriesServiceTest
    {
        private readonly ICountriesServices _services;
        private readonly Mock<ICountriesRepository> _countriesRepositoryMock;

        public CountriesServiceTest()
        {
            _countriesRepositoryMock = new Mock<ICountriesRepository>();
            _services = new CountriesServices(_countriesRepositoryMock.Object);
        }

        #region Add Country
        [Fact]
        public async Task AddCountry_RequestNull()
        {
            CountryAddRequest request = null;
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _services.AddCountry(request);
            });
        }

        [Fact]
        public async Task AddCountry_ArgumentNull()
        {
            CountryAddRequest request = new CountryAddRequest() { CountryName = null };
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _services.AddCountry(request);
            });
        }

        [Fact]
        public async Task AddCountry_Duplicated()
        {
            CountryAddRequest request1 = new CountryAddRequest() { CountryName = "USA" };

            _countriesRepositoryMock
                .Setup(repo => repo.GetCountryByName(It.IsAny<string>()))
                .ReturnsAsync(new Country { CountryName = "USA" });

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _services.AddCountry(request1);
            });
        }

        [Fact]
        public async Task AddCountry_True()
        {
            CountryAddRequest request = new CountryAddRequest() { CountryName = "USA" };

            _countriesRepositoryMock
                .Setup(repo => repo.GetCountryByName(It.IsAny<string>()))
                .ReturnsAsync((Country)null);  

            //_countriesRepositoryMock
            //    .Setup(repo => repo.AddCountry(It.IsAny<Country>()))
            //    .Returns(Task.CompletedTask);  

            CountryResponse countryResponse = await _services.AddCountry(request);
            Assert.NotNull(countryResponse);
            Assert.Equal("USA", countryResponse.CountryName);
        }

        #endregion

        #region Get All Country
        [Fact]
        public async Task GetAllCountry_ListEmpty()
        {
            _countriesRepositoryMock
                .Setup(repo => repo.GetAll())
                .ReturnsAsync(new List<Country>());

            var responseList = await _services.GetAllCountry();
            Assert.Empty(responseList);
        }

        [Fact]
        public async Task GetAllCountry_FewOutput()
        {
            var countries = new List<Country>
            {
                new Country { CountryId = Guid.NewGuid(), CountryName = "USA" },
                new Country { CountryId = Guid.NewGuid(), CountryName = "EGY" },
                new Country { CountryId = Guid.NewGuid(), CountryName = "KSA" }
            };

            _countriesRepositoryMock
                .Setup(repo => repo.GetAll())
                .ReturnsAsync(countries);

            List<CountryResponse> actualOutput = await _services.GetAllCountry();

            Assert.Equal(3, actualOutput.Count);
            Assert.Contains(actualOutput, c => c.CountryName == "USA");
            Assert.Contains(actualOutput, c => c.CountryName == "EGY");
            Assert.Contains(actualOutput, c => c.CountryName == "KSA");
        }
        #endregion

        #region Get By CountryId
        [Fact]
        public async Task GetByCountry_NullId()
        {
            Guid? countryId = null;
            var countryResponse = await _services.GetCountryBy(countryId);
            Assert.Null(countryResponse);
        }

        [Fact]
        public async Task GetByCountry_ValidationId()
        {
            var country = new Country { CountryId = Guid.NewGuid(), CountryName = "USA" };

            _countriesRepositoryMock
                .Setup(repo => repo.GetCountryById(It.IsAny<Guid>()))
                .ReturnsAsync(country);

            var countryResponse = await _services.GetCountryBy(country.CountryId);

            Assert.Equal(country.CountryId, countryResponse.CountryId);
            Assert.Equal(country.CountryName, countryResponse.CountryName);
        }
        #endregion
    }
}
