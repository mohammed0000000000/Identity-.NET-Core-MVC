using Microsoft.AspNetCore.Identity;

namespace IdentityDemo.Auth
{
    public class ApplicationUser:IdentityUser
    {
        public string Address { get; set; } 
    }
}
