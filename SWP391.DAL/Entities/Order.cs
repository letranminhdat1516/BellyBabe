using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public int? StatusId { get; set; }

    public string? Note { get; set; }

    public int? DeliveryId { get; set; }

    public int? VoucherId { get; set; }

    public int? TotalPrice { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? RecipientName { get; set; }

    public string? RecipientPhone { get; set; }

    public string? RecipientAddress { get; set; }

    public virtual Delivery? Delivery { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual OrderStatus? Status { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Voucher? Voucher { get; set; }
}
