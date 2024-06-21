using System;
using System.Collections.Generic;
using WebApplication1.Entities;

namespace SWP391.DAL.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public bool? IsSelling { get; set; }

    public string? MadeIn { get; set; }

    public string? Unit { get; set; }

    public string? SuitableAge { get; set; }

    public string? UsageInstructions { get; set; }

    public string? StorageInstructions { get; set; }

    public int Quantity { get; set; }

    public int IsSoldOut { get; set; }

    public DateTime? BackInStockDate { get; set; }

    public int? ManufacturerId { get; set; }

    public int? CategoryId { get; set; }

    public int? BrandId { get; set; }

    public int? Rating { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual ProductCategory? Category { get; set; }

    public virtual ICollection<CumulativeScoreTransaction> CumulativeScoreTransactions { get; set; } = new List<CumulativeScoreTransaction>();

    public virtual ICollection<CumulativeScore> CumulativeScores { get; set; } = new List<CumulativeScore>();

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<PreOrder> PreOrders { get; set; } = new List<PreOrder>();

    public virtual ICollection<PriceUpdate> PriceUpdates { get; set; } = new List<PriceUpdate>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
