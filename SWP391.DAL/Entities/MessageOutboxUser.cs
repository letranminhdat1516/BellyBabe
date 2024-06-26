using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class MessageOutboxUser
{
    public int OutboxId { get; set; }

    public int? FromUserId { get; set; }

    public int? ToUserId { get; set; }

    public int? MessageId { get; set; }

    public bool? IsView { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual ICollection<CustomerOption> CustomerOptions { get; set; } = new List<CustomerOption>();

    public virtual User? FromUser { get; set; }

    public virtual Message? Message { get; set; }

    public virtual User? ToUser { get; set; }
}
