using Microsoft.AspNetCore.Identity;


namespace ContactsManager.Core.Domain.IdentityEntites
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }
    }
}
