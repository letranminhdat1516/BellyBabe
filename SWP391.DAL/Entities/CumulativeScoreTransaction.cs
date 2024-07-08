using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class CumulativeScoreTransaction
{
    public int TransactionId { get; set; }

    public int? UserId { get; set; }

    public int? OrderId { get; set; }

    public int? ScoreChange { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string? TransactionType { get; set; }

    public virtual Order? Order { get; set; }

    public virtual User? User { get; set; }
}
