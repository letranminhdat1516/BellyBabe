using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class CumulativeScore
{
    public int ScoreId { get; set; }

    public int? UserId { get; set; }

    public int? TotalScore { get; set; }

    public int? AvailablePoints { get; set; }

    public int? RatingCount { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual User? User { get; set; }
}
