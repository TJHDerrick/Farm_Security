using System.ComponentModel.DataAnnotations;

namespace Fresh_Farm_Market_214142Z.ViewModels
{
    public class NewPassword
    {
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation does not match")]
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string token { get; set; }
    }
}
