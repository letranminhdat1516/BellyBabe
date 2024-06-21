using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.FeedbackRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.BLL.Services.FeedbackService
{
    public class FeedbackService
    {
        private readonly FeedbackRepository _feedbackRepository;

        public FeedbackService(FeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<Feedback> AddFeedbackAsync(string userName, int productId, string title, int rating)
        {
            var feedback = new Feedback
            {
                UserName = userName,
                ProductId = productId,
                Title = title,
                Rating = rating,
                DateCreated = DateTime.UtcNow
            };

            return await _feedbackRepository.AddFeedbackAsync(feedback);
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByProductIdAsync(int productId)
        {
            return await _feedbackRepository.GetFeedbacksByProductIdAsync(productId);
        }
    }

}
