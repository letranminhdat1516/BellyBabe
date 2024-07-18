using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public int? OrderId { get; set; }

    public int? Price { get; set; }

    public int? Quantity { get; set; }

    public bool? IsChecked { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }

}
