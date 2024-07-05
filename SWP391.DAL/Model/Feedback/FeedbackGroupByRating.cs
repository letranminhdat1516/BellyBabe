using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Model.Feedback
{
    public class FeedbackGroupByRating
    {
        public int Rating { get; set; }
        public List<FeedbackInfo> Feedbacks { get; set; }
    }
}
