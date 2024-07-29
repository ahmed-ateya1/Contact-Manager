using Model;
using System.Linq.Expressions;

namespace RepositoryContract
{
    public interface IPersonsRepository
    {
        Task<Person> AddPerson(Person Person);
        Task<Person?> GetPersonById(Guid PersonId);
        Task<List<Person>> GetAll();

        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

        Task<bool> DeletePersonById(Guid PersonId);
        Task<Person> UpdatePerson(Person person);

    }
}
