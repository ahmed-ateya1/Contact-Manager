using Microsoft.EntityFrameworkCore;
using Model;
using Model.AppDbContext;
using RepositoryContract;
using System.Linq.Expressions;


namespace Repository
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly PersonDbContext _db;

        public PersonsRepository(PersonDbContext db)
        {
            _db = db;
        }

        public async Task<Person> AddPerson(Person Person)
        {
            await _db.Persons.AddAsync(Person);
            await _db.SaveChangesAsync();
            return Person;
        }

        public async Task<bool> DeletePersonById(Guid personId)
        {
            _db.Persons.RemoveRange(_db.Persons.Where(x=>x.PersonId == personId));
            int rowDeleted = await _db.SaveChangesAsync();
            return rowDeleted > 0;
        }

        public async Task<List<Person>> GetAll()
        {
            return await _db.Persons.Include(x=>x.Country)
                .ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            return await _db.Persons
                .Include(x=>x.Country)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Person?> GetPersonById(Guid PersonId)
        {
            return await _db.Persons.
                FirstOrDefaultAsync(x=>x.PersonId == PersonId);
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            var matchingPerson = await GetPersonById(person.PersonId);
            if (matchingPerson == null)
                return person;

            matchingPerson.DateOfBirth = person.DateOfBirth;
            matchingPerson.Address = person.Address;
            matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;
            matchingPerson.CountryId = person.CountryId;
            matchingPerson.Email = person.Email;
            matchingPerson.Gender = person.Gender;
            matchingPerson.PersonName = person.PersonName;
            await _db.SaveChangesAsync();

            return matchingPerson;
        }
    }
}
