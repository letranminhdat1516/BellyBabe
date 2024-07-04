using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public string? Content { get; set; }

    public int? Rating { get; set; }

    public DateTime? DateCreated { get; set; }

    public int? RatingCategoryId { get; set; }

    public int? OrderDetailId { get; set; }

    public virtual ICollection<FeedbackResponse> FeedbackResponses { get; set; } = new List<FeedbackResponse>();

    public virtual OrderDetail? OrderDetail { get; set; }

    public virtual Product? Product { get; set; }

    public virtual RatingCategory? RatingCategory { get; set; }

    public virtual User? User { get; set; }
}
