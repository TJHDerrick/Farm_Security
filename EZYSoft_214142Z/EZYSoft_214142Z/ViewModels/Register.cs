using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;

namespace EZYSoft_214142Z.ViewModels
{
    public class Register
    {

        // Some refer from EDP Practical 2 or Microsoft materials

        //[Required]
        //public string FullName { get; set; } (Correct)


        //[Required]
        //[DataType(DataType.CreditCard)]
        //public string CreditCard { get; set; } (Most likely correct, dk if it is string or integer)


        //[Required]
        //public string Gender { get; set; } (Correct)


        //[Required]
        //[DataType(DataType.PhoneNumber)]
        //public string MobileNo { get; set; } (Most likely correct, dk need to put in [DataType(DataType.PhoneNumber)] or not)


        //[Required]
        //[DataType(DataType.PostalCode)]
        //public string Address { get; set; } (Most likely correct, dk if PostalCode is the best data type for it tho)


        //[Required]
        //[DataType(DataType.Upload)]
        //public string Photo { get; set; } ([DataType(DataType.Upload)] should be correct, dk what to put for datatype on line 38 tho)


        //[Required]
        //public string AboutMe { get; set; } (Most likely correct)

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Only alphabets can be put into names")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only numbers can be put into credit cards")]
        public string CreditCard { get; set; } = String.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only numbers can be put into phone numbers")]
        public string MobileNo { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[A-Za-z0-9 #-]*$", ErrorMessage = "Only numbers, letters and hashtags can be put into delivery addresses")]
        public string DeliveryAddress { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{12,}$",
            ErrorMessage = "Passwords must be at least 12 characters long and contain at least an uppercase letter, " +
            "lower case letter, digit and a symbol")]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }

        public string? ImageURL { get; set; } = "/uploads/user.jpg";

        public string AboutMe { get; set; }
    }
}
