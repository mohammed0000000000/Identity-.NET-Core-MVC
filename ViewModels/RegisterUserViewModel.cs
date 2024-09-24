using System.ComponentModel.DataAnnotations;

namespace IdentityDemo.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        [MinLength(4)]
        public string UserName { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ComfirmPassword { get; set; }


        public string Address { get; set; }

    }
}
