namespace SWP391.DAL.Model.Login
{
    public class UserLoginDTO
    {
        public string PhoneNumber { get; set; }
        public string OTP { get; set; }
        public DateTime OTPExpiry { get; set; }
    }
}
