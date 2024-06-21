using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class Feedback
{
    public int FeedbackId { get; set; }
    public string? UserName { get; set; }
    public string? Title { get; set; }
    public int? Rating { get; set; }
    public DateTime? DateCreated { get; set; }
    public int ProductId { get; set; }

    public virtual ICollection<FeedbackResponse> FeedbackResponses { get; set; } = new List<FeedbackResponse>();
    public virtual User? UserNameNavigation { get; set; }
    public virtual Product? Product { get; set; }
}

