using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Person
    {
        [Key]
        public Guid PersonId { get; set; }
        [StringLength(50)]//nvarchar(50)
        public string? PersonName {  get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(80)]
        public string? Address { get; set; }
        [StringLength(10)]
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public string? TIN {  get; set; }

        public Country? Country { get; set; }

    }
}
