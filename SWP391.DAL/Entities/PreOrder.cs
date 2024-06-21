using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class PreOrder
{
    public int PreOrderId { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public DateTime? PreOrderDate { get; set; }

    public bool? NotificationSent { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
