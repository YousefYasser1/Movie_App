using Microsoft.AspNetCore.Identity;

namespace Movies.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
