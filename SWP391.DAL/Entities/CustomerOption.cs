using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class CustomerOption
{
    public int CustomerOptionId { get; set; }

    public int UserId { get; set; }

    public int? MessageId { get; set; }

    public int? InboxId { get; set; }

    public int? OutboxId { get; set; }

    public string OptionValue { get; set; } = null!;

    public virtual MessageInboxUser? Inbox { get; set; }

    public virtual Message? Message { get; set; }

    public virtual MessageOutboxUser? Outbox { get; set; }

    public virtual User User { get; set; } = null!;
}
