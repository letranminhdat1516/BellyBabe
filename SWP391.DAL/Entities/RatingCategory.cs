using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities
{
    public partial class RatingCategory
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public int ProductId { get; set; }

        public int? TotalRatings { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        public virtual Product Product { get; set; } = null!;

        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
