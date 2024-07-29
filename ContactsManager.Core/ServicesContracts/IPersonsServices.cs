using ServiceContract.DTO;
using ServiceContract.Enumerator;

namespace ServiceContract
{
    public interface IPersonsServices
    {
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
        Task<PersonResponse?> GetPersonBy(Guid? personId);
        Task<List<PersonResponse>> GetAllPersons();

        Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);

        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> persons , string sortedBy , SortedOptions sortedOptions);

        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        Task<bool> DeletePerson(Guid? personId);
        Task<MemoryStream> GetPersonsCSV();
        Task<MemoryStream> GetPersonExcel();
    }
}
