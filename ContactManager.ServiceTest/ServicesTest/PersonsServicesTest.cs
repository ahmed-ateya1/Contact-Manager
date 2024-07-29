using Moq;
using Model;
using ServiceContract;
using ServiceContract.DTO;
using ServiceContract.Enumerator;
using Services;
using RepositoryContract;

namespace ServicesTest
{
    public class PersonsServicesTest
    {
        private readonly Mock<IPersonsRepository> _personsRepositoryMock;
        private readonly Mock<ICountriesServices> _countriesServicesMock;
        private readonly PersonsService _personsServices;

        public PersonsServicesTest()
        {
            _personsRepositoryMock = new Mock<IPersonsRepository>();
            _countriesServicesMock = new Mock<ICountriesServices>();

            _personsServices = new PersonsService(_personsRepositoryMock.Object, _countriesServicesMock.Object);
        }
        #region Add Person
        [Fact]
        public async Task AddPerson_NullArgument()
        {
            PersonAddRequest personAddRequest = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _personsServices.AddPerson(personAddRequest);
            });
        }

        [Fact]
        public async Task AddPerson_EmptyPersonName()
        {
            var personRequest = new PersonAddRequest() { PersonName = null };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personsServices.AddPerson(personRequest);
            });
        }

        [Fact]
        public async Task AddPerson_EmptyAddress()
        {
            var personRequest = new PersonAddRequest() { Address = null };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personsServices.AddPerson(personRequest);
            });
        }

        [Fact]
        public async Task AddPerson_Valid()
        {
            var countryAddRequest = new CountryAddRequest() { CountryName = "USA" };
            var countryResponse = new CountryResponse() { CountryId = Guid.NewGuid(), CountryName = "USA" };

            _countriesServicesMock.Setup(service => service.AddCountry(It.IsAny<CountryAddRequest>()))
                .ReturnsAsync(countryResponse);

            var personAddRequest = new PersonAddRequest()
            {
                Address = "cairo",
                CountryId = countryResponse.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "ahmed@gmail.com",
                Gender = GenderOptions.MALE,
                PersonName = "ahmed",
                ReceiveNewsLetters = true,
            };
            var personResponse = await _personsServices.AddPerson(personAddRequest);

            Assert.True(personResponse.PersonId != Guid.Empty);
        }
        #endregion

        #region Get Person By Id
        [Fact]
        public async Task GetPerson_InvalidId()
        {
            Guid? personId = null;

            var personResponseGet = await _personsServices.GetPersonBy(personId);
            Assert.Null(personResponseGet);
        }

        [Fact]
        public async Task GetPerson_ValidId()
        {
            var countryAddRequest = new CountryAddRequest() { CountryName = "USA" };
            var countryResponse = new CountryResponse() { CountryId = Guid.NewGuid(), CountryName = "USA" };

            _countriesServicesMock.Setup(service => service.AddCountry(It.IsAny<CountryAddRequest>()))
                .ReturnsAsync(countryResponse);

            var personAddRequest = new PersonAddRequest()
            {
                Address = "cairo",
                CountryId = countryResponse.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "ahmed@gmail.com",
                Gender = GenderOptions.MALE,
                PersonName = "ahmed",
                ReceiveNewsLetters = true,
            };
            var personResponse = await _personsServices.AddPerson(personAddRequest);

            var personResponseGetBy = await _personsServices.GetPersonBy(personResponse.PersonId);

            Assert.Equal(personResponse.PersonId, personResponseGetBy.PersonId);
        }
        #endregion

        #region Get All Persons
        [Fact]
        public async Task GetAllPersons_Empty()
        {
            var persons = await _personsServices.GetAllPersons();
            Assert.Empty(persons);
        }

        [Fact]
        public async Task GetAllPersons_FewPersons()
        {
            var countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
            var countryAddRequest2 = new CountryAddRequest() { CountryName = "KSA" };

            var countryResponse1 = new CountryResponse() { CountryId = Guid.NewGuid(), CountryName = "USA" };
            var countryResponse2 = new CountryResponse() { CountryId = Guid.NewGuid(), CountryName = "KSA" };

            _countriesServicesMock.Setup(service => service.AddCountry(It.IsAny<CountryAddRequest>()))
                .ReturnsAsync(countryResponse1);
            _countriesServicesMock.Setup(service => service.AddCountry(It.IsAny<CountryAddRequest>()))
                .ReturnsAsync(countryResponse2);

            var personAddRequest1 = new PersonAddRequest()
            {
                Address = "cairo",
                CountryId = countryResponse1.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "ahmed@gmail.com",
                Gender = GenderOptions.MALE,
                PersonName = "ahmed",
                ReceiveNewsLetters = true,
            };
            var personAddRequest2 = new PersonAddRequest()
            {
                Address = "cairo",
                CountryId = countryResponse2.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "ahmed@gmail.com",
                Gender = GenderOptions.MALE,
                PersonName = "ahmed",
                ReceiveNewsLetters = true,
            };

            var personAddRequests = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2 };

            foreach (var request in personAddRequests)
            {
                await _personsServices.AddPerson(request);
            }

            var personResponseGetAll = await _personsServices.GetAllPersons();

            Assert.Equal(2, personResponseGetAll.Count);
        }
        #endregion

        #region Get Filtered Persons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchString()
        {
            var countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
            var countryAddRequest2 = new CountryAddRequest() { CountryName = "KSA" };

            var countryResponse1 = new CountryResponse() { CountryId = Guid.NewGuid(), CountryName = "USA" };
            var countryResponse2 = new CountryResponse() { CountryId = Guid.NewGuid(), CountryName = "KSA" };

            _countriesServicesMock.Setup(service => service.AddCountry(It.IsAny<CountryAddRequest>()))
                .ReturnsAsync(countryResponse1);
            _countriesServicesMock.Setup(service => service.AddCountry(It.IsAny<CountryAddRequest>()))
                .ReturnsAsync(countryResponse2);

            var personAddRequest1 = new PersonAddRequest()
            {
                Address = "cairo",
                CountryId = countryResponse1.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "ahmed@gmail.com",
                Gender = GenderOptions.MALE,
                PersonName = "ahmed",
                ReceiveNewsLetters = true,
            };
            var personAddRequest2 = new PersonAddRequest()
            {
                Address = "cairo",
                CountryId = countryResponse1.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "ahmed@gmail.com",
                Gender = GenderOptions.MALE,
                PersonName = "aahmed",
                ReceiveNewsLetters = true,
            };

            var personAddRequests = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2 };

            foreach (var request in personAddRequests)
            {
                await _personsServices.AddPerson(request);
            }

            var personResponseGetFiltered = await _personsServices.GetFilteredPersons(nameof(Person.PersonName), "");

            Assert.Equal(2, personResponseGetFiltered.Count);
        }

        [Fact]
        public async Task GetFilteredPersons_SearchPersonName()
        {
            var countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
            var countryAddRequest2 = new CountryAddRequest() { CountryName = "KSA" };

            var countryResponse1 = new CountryResponse() { CountryId = Guid.NewGuid(), CountryName = "USA" };
            var countryResponse2 = new CountryResponse() { CountryId = Guid.NewGuid(), CountryName = "KSA" };

            _countriesServicesMock.Setup(service => service.AddCountry(It.IsAny<CountryAddRequest>()))
                .ReturnsAsync(countryResponse1);
            _countriesServicesMock.Setup(service => service.AddCountry(It.IsAny<CountryAddRequest>()))
                .ReturnsAsync(countryResponse2);

            var personAddRequest1 = new PersonAddRequest()
            {
                Address = "cairo",
                CountryId = countryResponse1.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "ahmed@gmail.com",
                Gender = GenderOptions.MALE,
                PersonName = "ahmed",
                ReceiveNewsLetters = true,
            };
            var personAddRequest2 = new PersonAddRequest()
            {
                Address = "cairo",
                CountryId = countryResponse1.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "ahmed@gmail.com",
                Gender = GenderOptions.MALE,
                PersonName = "aahmed",
                ReceiveNewsLetters = true,
            };

            var personAddRequests = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2 };

            foreach (var request in personAddRequests)
            {
                await _personsServices.AddPerson(request);
            }

            var personResponseGetFiltered = await _personsServices.GetFilteredPersons(nameof(Person.PersonName), "ah");

            Assert.Single(personResponseGetFiltered);
        }
        #endregion

        #region Get Sorted Persons
        [Fact]
        public async Task GetSortedPersons_Valid()
        {
            var countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
            var countryAddRequest2 = new CountryAddRequest() { CountryName = "KSA" };

            var countryResponse1 = new CountryResponse() { CountryId = Guid.NewGuid(), CountryName = "USA" };
            var countryResponse2 = new CountryResponse() { CountryId = Guid.NewGuid(), CountryName = "KSA" };

            _countriesServicesMock.Setup(service => service.AddCountry(It.IsAny<CountryAddRequest>()))
                .ReturnsAsync(countryResponse1);
            _countriesServicesMock.Setup(service => service.AddCountry(It.IsAny<CountryAddRequest>()))
                .ReturnsAsync(countryResponse2);

            var personAddRequest1 = new PersonAddRequest()
            {
                Address = "cairo",
                CountryId = countryResponse1.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "ahmed@gmail.com",
                Gender = GenderOptions.MALE,
                PersonName = "ahmed",
                ReceiveNewsLetters = true,
            };
            var personAddRequest2 = new PersonAddRequest()
            {
                Address = "cairo",
                CountryId = countryResponse1.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "ahmed@gmail.com",
                Gender = GenderOptions.MALE,
                PersonName = "ahmed",
                ReceiveNewsLetters = true,
            };

            var personAddRequests = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2 };

            foreach (var request in personAddRequests)
            {
                await _personsServices.AddPerson(request);
            }

            var personResponseGetSortedDESC = await _personsServices.GetSortedPersons(
                await _personsServices.GetAllPersons(), nameof(Person.PersonName), SortedOptions.DESC);

            var personOrderDesc = (await _personsServices.GetAllPersons()).OrderByDescending(x => x.PersonName).ToList();

            for (int i = 0; i < personResponseGetSortedDESC.Count(); i++)
            {
                Assert.Equal(personOrderDesc[i].PersonId, personResponseGetSortedDESC[i].PersonId);
            }
        }
        #endregion

        #region Update Person
        [Fact]
        public async Task UpdatePerson_NullArgument()
        {
            PersonUpdateRequest? personUpdate = null;
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _personsServices.UpdatePerson(personUpdate);
            });
        }

        [Fact]
        public async Task UpdatePerson_InvalidPersonId()
        {
            PersonUpdateRequest? personUpdate = new PersonUpdateRequest()
            { PersonId = Guid.NewGuid() };
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personsServices.UpdatePerson(personUpdate);
            });
        }

        [Fact]
        public async Task UpdatePerson_InvalidPersonName()
        {
            var countryAddRequest = new CountryAddRequest() { CountryName = "UK" };
            var countryResponse = new CountryResponse() { CountryId = Guid.NewGuid(), CountryName = "UK" };

            _countriesServicesMock.Setup(service => service.AddCountry(It.IsAny<CountryAddRequest>()))
                .ReturnsAsync(countryResponse);

            var personAddRequest = new PersonAddRequest()
            {
                PersonName = "ahmed",
                CountryId = countryResponse.CountryId,
                Email = "ahmed@gmail.com",
                DateOfBirth = DateTime.Parse("2000-02-02"),
                Gender = GenderOptions.MALE,
                Address = "cairo",
                ReceiveNewsLetters = true
            };

            var personResponse = await _personsServices.AddPerson(personAddRequest);

            var personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personsServices.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public async Task UpdatePerson_FullDetailsUpdate()
        {
            var countryAddRequest = new CountryAddRequest() { CountryName = "UK" };
            var countryResponse = new CountryResponse() { CountryId = Guid.NewGuid(), CountryName = "UK" };

            _countriesServicesMock.Setup(service => service.AddCountry(It.IsAny<CountryAddRequest>()))
                .ReturnsAsync(countryResponse);

            var personAddRequest = new PersonAddRequest()
            {
                PersonName = "ahmed",
                CountryId = countryResponse.CountryId,
                Address = "cairo",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "aje@gmail.com",
                Gender = GenderOptions.MALE,
                ReceiveNewsLetters = true
            };

            var personResponse = await _personsServices.AddPerson(personAddRequest);

            var personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "ali";
            personUpdateRequest.Email = "ahmm@gmail.com";

            var personUpdateFromUpdate = await _personsServices.UpdatePerson(personUpdateRequest);
            var personUpdateFromGet = await _personsServices.GetPersonBy(personUpdateRequest.PersonId);

            Assert.Equal(personUpdateFromUpdate.PersonId, personUpdateFromGet.PersonId);
        }
        #endregion

        #region Delete Person
        [Fact]
        public async Task DeletePerson_ValidId()
        {
            var countryAddRequest = new CountryAddRequest() { CountryName = "UK" };
            var countryResponse = new CountryResponse() { CountryId = Guid.NewGuid(), CountryName = "UK" };

            _countriesServicesMock.Setup(service => service.AddCountry(It.IsAny<CountryAddRequest>()))
                .ReturnsAsync(countryResponse);

            var personAddRequest = new PersonAddRequest()
            {
                PersonName = "ahmed",
                CountryId = countryResponse.CountryId,
                Email = "ahmed@gmail.com",
                DateOfBirth = DateTime.Parse("2000-02-02"),
                Gender = GenderOptions.MALE,
                Address = "cairo",
                ReceiveNewsLetters = true
            };

            var personResponse = await _personsServices.AddPerson(personAddRequest);

            var isDeleted = await _personsServices.DeletePerson(personResponse.PersonId);

            Assert.True(isDeleted);
        }

        [Fact]
        public async Task DeletePerson_InvalidId()
        {
            Guid personId = Guid.NewGuid();

            bool isDeleted = await _personsServices.DeletePerson(personId);

            Assert.False(isDeleted);
        }
        #endregion
    }
}
