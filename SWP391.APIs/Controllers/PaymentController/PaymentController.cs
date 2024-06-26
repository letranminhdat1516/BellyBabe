using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services.PaymentService;
using SWP391.DAL.Entities.payment;
using SWP391.DAL.Entities.payment.Response;
using System.Security.Claims;

namespace SWP391.APIs.Controllers.PaymentController
{

        [ApiController]
        [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly VnpayService _vnpayService;

        public PaymentController(IHttpContextAccessor httpContextAccessor, VnpayService vnpayService)
        {
            _httpContextAccessor = httpContextAccessor;
            _vnpayService = vnpayService;
        }


        //Create payment
        [HttpPost]
        public IActionResult CreatePayment([FromBody] PaymentDtos paymentDtos)
        {
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            string? ipAddress = GetClientIpAddress(_httpContextAccessor.HttpContext);
            var result = _vnpayService.CreatePayment(paymentDtos, userId, ipAddress);
            return Ok(result);
        }

        private string? GetClientIpAddress(HttpContext httpContext)
        {
            // Check for proxy headers
            string? ipAddress = httpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(ipAddress))
            {
                // Use the IP address from the proxy header
                return ipAddress;
            }
            else
            {
                // Use the local IP address
                return httpContext?.Connection?.LocalIpAddress?.ToString();
            }
        }

        //Check payment response
        [HttpGet]
        public IActionResult CheckPaymentResponse([FromQuery] VnpayPayResponse vnpayResponse)
        {
            var result = _vnpayService.CheckPaymentResponse(vnpayResponse);
            return Ok(result);
        }

    }
    }

