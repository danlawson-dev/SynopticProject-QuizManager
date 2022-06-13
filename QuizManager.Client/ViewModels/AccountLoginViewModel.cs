using System.ComponentModel.DataAnnotations;

namespace QuizManager.Client.ViewModels
{
    public class AccountLoginViewModel
    {
        public string ReturnUrl { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberLogin { get; set; }
    }
}