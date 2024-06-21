﻿using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class Delivery
{
    public int DeliveryId { get; set; }

    public string DeliveryName { get; set; } = null!;

    public string DeliveryMethod { get; set; } = null!;

    public decimal DeliveryFee { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
