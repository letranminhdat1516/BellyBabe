using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class PriceUpdate
{
    public int PriceId { get; set; }

    public decimal? Price { get; set; }

    public DateTime? DateApplied { get; set; }

    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }
}
