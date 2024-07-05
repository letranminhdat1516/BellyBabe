using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class OrderStatus
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public int? OrderId { get; set; }

    public DateTime? StatusUpdateDate { get; set; }

    public virtual Order? Order { get; set; }

}
