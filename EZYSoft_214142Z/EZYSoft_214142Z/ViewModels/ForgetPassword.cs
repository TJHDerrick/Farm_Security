using System.ComponentModel.DataAnnotations;

namespace Fresh_Farm_Market_214142Z.ViewModels
{
    public class ForgetPassword
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
