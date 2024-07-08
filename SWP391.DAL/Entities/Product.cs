using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public bool? IsSelling { get; set; }

    public string? Description { get; set; }

    public int Quantity { get; set; }

    public int IsSoldOut { get; set; }

    public DateTime? BackInStockDate { get; set; }

    public int? CategoryId { get; set; }

    public int? BrandId { get; set; }

    public int? FeedbackTotal { get; set; }

    public int? OldPrice { get; set; }

    public decimal? Discount { get; set; }

    public decimal? NewPrice { get; set; }

    public string? ImageLinks { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual ProductCategory? Category { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<PreOrder> PreOrders { get; set; } = new List<PreOrder>();

    public virtual ICollection<RatingCategory> RatingCategories { get; set; } = new List<RatingCategory>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Statistic> Statistics { get; set; } = new List<Statistic>();

}
