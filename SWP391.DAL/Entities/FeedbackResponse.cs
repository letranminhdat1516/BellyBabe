using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class FeedbackResponse
{
    public int ResponseId { get; set; }

    public int? FeedbackId { get; set; }

    public int? UserId { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual Feedback? Feedback { get; set; }

    public virtual User? User { get; set; }
}
