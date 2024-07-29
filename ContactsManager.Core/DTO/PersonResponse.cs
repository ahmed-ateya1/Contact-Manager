using Model;
using ServiceContract.Enumerator;

namespace ServiceContract.DTO
{
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? Country { get; set; }
        public Guid? CountryId { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PersonResponse response &&
                   PersonId.Equals(response.PersonId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PersonId);
        }
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                Address = Address,
                CountryId = CountryId,
                DateOfBirth = DateOfBirth,
                Email = Email,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
                PersonName = PersonName,
                ReceiveNewsLetters = ReceiveNewsLetters,
                PersonId = PersonId,
            };
        }
    }
    public static class PersonExtension
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                Address = person.Address,
                CountryId = person.CountryId,
                DateOfBirth = person.DateOfBirth,
                Email = person.Email,
                PersonName = person.PersonName,
                PersonId = person.PersonId,
                Gender = person.Gender,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = (person.DateOfBirth != null) ?
                Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null,
                Country = person.Country?.CountryName
            };
        }
    }
}
