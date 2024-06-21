using System;
using System.Collections.Generic;
using WebApplication1.Entities;

namespace SWP391.DAL.Entities;

public partial class CumulativeScore
{
    public int ScoreId { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public decimal? TotalScore { get; set; }

    public int? RatingCount { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual ICollection<CumulativeScoreTransaction> CumulativeScoreTransactions { get; set; } = new List<CumulativeScoreTransaction>();

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
