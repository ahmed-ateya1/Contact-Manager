using Model;


namespace ServiceContract.DTO
{
    public class CountryResponse
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CountryResponse response &&
                   CountryId.Equals(response.CountryId) &&
                   CountryName == response.CountryName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CountryId, CountryName);
        }
    }
    public static class CountryExtension
    {
        public static CountryResponse ToCountry(this Country country)
        {
            return new CountryResponse()
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName
            };
        }
    }
}
