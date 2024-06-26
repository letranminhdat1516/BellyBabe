using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? UserId { get; set; }

    public string? Content { get; set; }

    public int? Rating { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual ICollection<FeedbackResponse> FeedbackResponses { get; set; } = new List<FeedbackResponse>();

    public virtual User? User { get; set; }
}
