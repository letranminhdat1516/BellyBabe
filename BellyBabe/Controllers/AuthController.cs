using BellyBabe.Entity;
using BellyBabe.Repository;
using BellyBabe.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace BellyBabe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly OtpService _otpService;

        public AuthController(IUserRepository userRepository, OtpService otpService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _otpService = otpService ?? throw new ArgumentNullException(nameof(otpService));
        }


        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOtp([FromBody] string phoneNumber)
        {
            var otp = _otpService.GenerateOtp();
            var user = await _userRepository.GetUserByPhoneNumberAsync(phoneNumber);
            if (user == null)
            {
                user = new UserModel
                {
                    PhoneNumber = phoneNumber,
                    OTP = otp,
                    OTPExpiry = DateTime.UtcNow.AddMinutes(5)
                };
                await _userRepository.AddUserAsync(user);
            }
            else
            {
                user.OTP = otp;
                user.OTPExpiry = DateTime.UtcNow.AddMinutes(5);
                await _userRepository.UpdateUserAsync(user);
            }

            // call API Stringee
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-STRINGEE-AUTH", "eyJjdHkiOiJzdHJpbmdlZS1hcGk7dj0xIiwidHlwIjoiSldUIiwiYWxnIjoiSFMyNTYifQ.eyJqdGkiOiJTSy4wLjZjVUVESVVUWVgwNFRkYzZOemZTVXowdkNJNFlBTHUtMTcxNjIxODA5NyIsImlzcyI6IlNLLjAuNmNVRURJVVRZWDA0VGRjNk56ZlNVejB2Q0k0WUFMdSIsImV4cCI6MTcxODgxMDA5NywicmVzdF9hcGkiOnRydWV9.THIS1dAmtoJY1lGKR--c85pFStiO-2sWne9BkPw9WGA");

                var body = new
                {
                    from = new
                    {
                        type = "external",
                        number = "842471015037",
                        alias = "STRINGEE_NUMBER"
                    },
                    to = new[]
                    {
                        new
                        {
                            type = "external",
                            number = phoneNumber,
                            alias = "TO_NUMBER"
                        }
                    },
                    answer_url = "https://example.com/answerurl",
                    actions = new[]
                    {
                        new
                        {
                            action = "talk",
                            text = $"MÃ XÁC THỰC CỦA BẠN LÀ {otp}"
                        }
                    }
                };

                var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://api.stringee.com/v1/call2/callout", content);

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
            }

            return Ok(new { OTP = otp });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            var user = await _userRepository.GetUserByPhoneNumberAsync(request.PhoneNumber);
            if (user == null || user.OTP != request.OTP || user.OTPExpiry < DateTime.UtcNow)
            {
                return Unauthorized("Invalid OTP or OTP expired");
            }
            return Ok("Login successful");
        }

        public class VerifyOtpRequest
        {
            public string PhoneNumber { get; set; }
            public string OTP { get; set; }
        }

    }
}
