using Model;
using ServiceContract.DTO;
using ServiceContract;
using ServiceContract.Enumerator;
using Services.ValidationHelper;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using OfficeOpenXml;
using RepositoryContract;

namespace Services
{
    public class PersonsService : IPersonsServices
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ICountriesServices _countriesServices;

        public PersonsService(IPersonsRepository personsRepository, ICountriesServices countriesServices)
        {
            _personsRepository = personsRepository;
            _countriesServices = countriesServices;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            //check if PersonAddRequest is not null
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Model validation
            ValidateHelper.ValidateModel(personAddRequest);

            //convert personAddRequest into Person type
            Person person = personAddRequest.ToPerson();

            //generate PersonID
            person.PersonId = Guid.NewGuid();

            //add person object to persons list
            await _personsRepository.AddPerson(person);

            //convert the Person object into PersonResponse type
            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            var persons = await _personsRepository.GetAll();
            return persons.Select(x=> x.ToPersonResponse())
                .ToList(); 
        }

        public async Task<PersonResponse?> GetPersonBy(Guid? personID)
        {
            if (personID == null)
                return null;

            Person? person = await _personsRepository.GetPersonById(personID.Value);

            if (person == null)
                return null;

            return PersonExtension.ToPersonResponse(person);
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<Person> persons = searchBy switch
            {
                nameof(PersonResponse.PersonName) =>
                 await _personsRepository.GetFilteredPersons(temp =>
                 temp.PersonName.Contains(searchString)),

                nameof(PersonResponse.Email) =>
                 await _personsRepository.GetFilteredPersons(temp =>
                 temp.Email.Contains(searchString)),

                nameof(PersonResponse.DateOfBirth) =>
                 await _personsRepository.GetFilteredPersons(temp =>
                 temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString)),


                nameof(PersonResponse.Gender) =>
                 await _personsRepository.GetFilteredPersons(temp =>
                 temp.Gender.Contains(searchString)),

                nameof(PersonResponse.CountryId) =>
                 await _personsRepository.GetFilteredPersons(temp =>
                 temp.Country.CountryName.Contains(searchString)),

                nameof(PersonResponse.Address) =>
                await _personsRepository.GetFilteredPersons(temp =>
                temp.Address.Contains(searchString)),

                _ => await _personsRepository.GetAll()
            };
            return persons.Select(temp => temp.ToPersonResponse()).ToList();

        }


        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortedOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allPersons;

            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortedOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.PersonName), SortedOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortedOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortedOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortedOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortedOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),
                (nameof(PersonResponse.Age), SortedOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),
                (nameof(PersonResponse.Age), SortedOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),
                (nameof(PersonResponse.Gender), SortedOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Gender), SortedOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Country), SortedOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Country), SortedOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), SortedOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), SortedOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortedOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortedOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),
                _ => allPersons
            };

            return sortedPersons;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
                throw new ArgumentNullException(nameof(Person));


            ValidateHelper.ValidateModel(personUpdateRequest);
            var matchingPerson = await _personsRepository.GetPersonById(personUpdateRequest.PersonId);
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            matchingPerson.CountryId = personUpdateRequest.CountryId;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.PersonName = personUpdateRequest.PersonName;

            await _personsRepository.UpdatePerson(matchingPerson);

            return matchingPerson.ToPersonResponse();
        }

        public async Task<bool> DeletePerson(Guid? personID)
        {
            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID));
            }

            Person? person = await _personsRepository
                .GetPersonById(personID.Value);

            if (person == null)
                return false;

          await _personsRepository.DeletePersonById(personID.Value);

            return true;
        }
        public async Task<MemoryStream> GetPersonsCSV()
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture);

            var csvWriter = new CsvWriter(streamWriter , csvConfig);

            csvWriter.WriteField(nameof(PersonResponse.PersonName));
            csvWriter.WriteField(nameof(PersonResponse.Country));

            csvWriter.NextRecord(); 

            var persons = (await _personsRepository.GetAll())
                    .Select(x => x.ToPersonResponse());

            foreach (var person in persons)
            {
                csvWriter.WriteField(person.PersonName);
                csvWriter.WriteField(person.Country);
                csvWriter.NextRecord();
                csvWriter.Flush();
            }
            memoryStream.Position = 0;
            return memoryStream;
        }

        public async Task<MemoryStream> GetPersonExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var memoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PersonInfo");
                worksheet.Cells["A1"].Value = nameof(PersonResponse.PersonName);
                worksheet.Cells["B1"].Value = nameof(PersonResponse.Address);
                worksheet.Cells["C1"].Value = nameof(PersonResponse.Country);
                worksheet.Cells["D1"].Value = nameof(PersonResponse.Age);
                worksheet.Cells["E1"].Value = nameof(PersonResponse.Email);
                worksheet.Cells["F1"].Value = nameof(PersonResponse.DateOfBirth);
                worksheet.Cells["G1"].Value = nameof(PersonResponse.ReceiveNewsLetters);
                worksheet.Cells["H1"].Value = nameof(PersonResponse.Gender);

                int row = 2;
                var persons = (await _personsRepository.GetAll())
                    .Select(x => x.ToPersonResponse());

                foreach (var person in persons)
                {
                    worksheet.Cells[row, 1].Value = person.PersonName;
                    worksheet.Cells[row, 2].Value = person.Address;
                    worksheet.Cells[row, 3].Value = person.Country;
                    worksheet.Cells[row, 4].Value = person.Age;
                    worksheet.Cells[row, 5].Value = person.Email;
                    worksheet.Cells[row, 6].Value = person.DateOfBirth?.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 7].Value = person.ReceiveNewsLetters;
                    worksheet.Cells[row, 8].Value = person.Gender;
                    row++;
                }

                worksheet.Cells[$"A1:H{row - 1}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }
            memoryStream.Position = 0;
            return memoryStream;
        }

    }

}
