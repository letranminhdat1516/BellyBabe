using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public string? Note { get; set; }

    public int? VoucherId { get; set; }

    public int? TotalPrice { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? RecipientName { get; set; }

    public string? RecipientPhone { get; set; }

    public string? RecipientAddress { get; set; }

    public int? PointsUsed { get; set; }

    public virtual ICollection<CumulativeScoreTransaction> CumulativeScoreTransactions { get; set; } = new List<CumulativeScoreTransaction>();

    public virtual ICollection<DeliveryMethod> DeliveryMethods { get; set; } = new List<DeliveryMethod>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<OrderStatus> OrderStatuses { get; set; } = new List<OrderStatus>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Statistic> Statistics { get; set; } = new List<Statistic>();

    public virtual User User { get; set; } = null!;

    public virtual Voucher? Voucher { get; set; }

}
