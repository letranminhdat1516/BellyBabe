using Microsoft.AspNetCore.Identity;


namespace BellyBabe.Entity
{
    public class UserModel : IdentityUser
    {
        public int UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string OTP { get; set; }
        public DateTime OTPExpiry { get; set; }

    }
}
