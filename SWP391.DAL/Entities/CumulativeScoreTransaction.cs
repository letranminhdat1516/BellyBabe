using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class CumulativeScoreTransaction
{
    public int TransactionId { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public decimal? ScoreChange { get; set; }

    public DateTime? TransactionDate { get; set; }

    public int? ScoreId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual CumulativeScore? Score { get; set; }

    public virtual User? User { get; set; }
}
