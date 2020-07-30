using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LoanManager.Web
{ 
    public class LoginViewModel
    {
        [Required]
        [DisplayName("User name")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}

