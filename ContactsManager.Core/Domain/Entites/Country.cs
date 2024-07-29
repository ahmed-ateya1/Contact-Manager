using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Country
    {
        [Key]
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }
        public virtual ICollection<Person> Persons { get; set; } = new List<Person>();

    }
}
