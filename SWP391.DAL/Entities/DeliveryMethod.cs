using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class DeliveryMethod
{
    public int DeliveryId { get; set; }

    public string DeliveryName { get; set; } = null!;

    public int? DeliveryFee { get; set; }

    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }
}
