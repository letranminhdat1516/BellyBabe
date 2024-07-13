using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class Voucher
{
    public int VoucherId { get; set; }

    public string VoucherName { get; set; } = null!;

    public string? VoucherCode { get; set; }

    public int? Quantity { get; set; }

    public DateTime? ExpiredDate { get; set; }

    public int? UserId { get; set; }
    public decimal? Price { get; set; }
    public decimal? MinimumBillAmount { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User? User { get; set; }
}
