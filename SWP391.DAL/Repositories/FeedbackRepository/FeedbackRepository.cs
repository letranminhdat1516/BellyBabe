using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.FeedbackRepository
{
    public class FeedbackRepository
    {
        private readonly Swp391Context _context;

        public FeedbackRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task<Feedback> AddFeedbackAsync(Feedback feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByProductIdAsync(int productId)
        {
            return await _context.Feedbacks
                .Where(f => f.ProductId == productId)
                .ToListAsync();
        }
    }

}
