using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace BellyBabe.Service
{
    public class OtpService
    {
        private readonly ILogger<OtpService> _logger;

        public OtpService(ILogger<OtpService> logger)
        {
            _logger = logger;
        }

        public string GenerateOtp()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var byteArray = new byte[2];
                rng.GetBytes(byteArray);
                var otp = BitConverter.ToUInt16(byteArray, 0) % 10000; // Generate a number between 0 and 9999
                var otpString = otp.ToString("D4");
                _logger.LogInformation($"Generated OTP: {otpString}");
                return otpString;
            }
        }
    }
}
