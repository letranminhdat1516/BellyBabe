using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Model.Feedback
{
    public class FeedbackInfo
    {
        public string Content { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
