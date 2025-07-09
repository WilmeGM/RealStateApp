using Microsoft.AspNetCore.Identity;

namespace RealStateApp.Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhotoUrl { get; set; }
        public string? IdCard { get; set; }
        public bool IsActive { get; set; }
    }
}
