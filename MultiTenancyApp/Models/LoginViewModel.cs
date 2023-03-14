using System.ComponentModel.DataAnnotations;

namespace MultiTenancyApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "{0} required")]
        [EmailAddress(ErrorMessage = "{0} invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remenber me")]
        public bool RememberMe { get; set; }
    }
}
