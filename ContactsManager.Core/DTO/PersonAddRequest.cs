using Model;
using ServiceContract.Enumerator;
using System.ComponentModel.DataAnnotations;

namespace ServiceContract.DTO
{
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "Person name is required.")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public GenderOptions? Gender { get; set; }

        public Guid? CountryId { get; set; }

        public bool ReceiveNewsLetters { get; set; }

        public Person ToPerson()
        {
            return new Person()
            {
                Address = Address,
                CountryId = CountryId,
                DateOfBirth = DateOfBirth,
                Email = Email,
                Gender = Gender.ToString(),
                PersonName = PersonName,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
    }
}
