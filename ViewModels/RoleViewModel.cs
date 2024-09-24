using Microsoft.Build.Framework;

namespace IdentityDemo.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
