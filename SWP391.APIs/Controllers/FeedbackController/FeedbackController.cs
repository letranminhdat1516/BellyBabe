using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services.FeedbackService;

namespace SWP391.APIs.Controllers.FeedbackController
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;

        public FeedbackController(FeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFeedback([FromBody] AddFeedbackModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid client request");
            }

            var feedback = await _feedbackService.AddFeedbackAsync(model.UserName, model.ProductId, model.Title, model.Rating);
            return Ok(feedback);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetFeedbacksByProductId(int productId)
        {
            var feedbacks = await _feedbackService.GetFeedbacksByProductIdAsync(productId);
            return Ok(feedbacks);
        }
    }

    public class AddFeedbackModel
    {
        public string UserName { get; set; }
        public int ProductId { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
    }

}
