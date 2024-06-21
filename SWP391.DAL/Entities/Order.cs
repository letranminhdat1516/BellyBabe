using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public string? Address { get; set; }

    public int? StatusId { get; set; }

    public string? Note { get; set; }

    public int? DeliveryId { get; set; }

    public int? VoucherId { get; set; }

    public string? DeliveryMethod { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? PhoneNumber { get; set; }

    public decimal DeliveryFee { get; set; }

    public bool? Checked { get; set; }

    public virtual Delivery? Delivery { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual OrderStatus? Status { get; set; }

    public virtual User? User { get; set; }

    public virtual Voucher? Voucher { get; set; }
}
