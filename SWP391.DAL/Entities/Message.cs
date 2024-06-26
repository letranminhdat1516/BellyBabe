using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class Message
{
    public int MessageId { get; set; }

    public int? UserId { get; set; }

    public string? Title { get; set; }

    public string? MessageContent { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual ICollection<CustomerOption> CustomerOptions { get; set; } = new List<CustomerOption>();

    public virtual ICollection<MessageInboxUser> MessageInboxUsers { get; set; } = new List<MessageInboxUser>();

    public virtual ICollection<MessageOutboxUser> MessageOutboxUsers { get; set; } = new List<MessageOutboxUser>();

    public virtual User? User { get; set; }
}
