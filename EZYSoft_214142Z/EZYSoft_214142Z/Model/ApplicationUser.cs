using Microsoft.AspNetCore.Identity;

namespace Fresh_Farm_Market_214142Z.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = String.Empty;

        public string CreditCard { get; set; } = String.Empty;

        public string Gender { get; set; } = String.Empty;

        public string MobileNo { get; set; } = String.Empty;

        public string DeliveryAddress { get; set; } = String.Empty;

        public string? ImageURL { get; set; } = String.Empty;

        public string AboutMe { get; set; } = String.Empty;

    }
}
