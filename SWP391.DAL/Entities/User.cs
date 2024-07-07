using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public int? RoleId { get; set; }

    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? FullName { get; set; }

    public int? CumulativeScore { get; set; }

    public string? Otp { get; set; }

    public DateTime? Otpexpiry { get; set; }

    public bool? IsActive { get; set; }
    public string? Image { get; set; }
    public bool IsFirstLogin { get; set; }
    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<CumulativeScoreTransaction> CumulativeScoreTransactions { get; set; } = new List<CumulativeScoreTransaction>();

    public virtual ICollection<CumulativeScore> CumulativeScores { get; set; } = new List<CumulativeScore>();

    public virtual ICollection<CustomerOption> CustomerOptions { get; set; } = new List<CustomerOption>();

    public virtual ICollection<FeedbackResponse> FeedbackResponses { get; set; } = new List<FeedbackResponse>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<MessageInboxUser> MessageInboxUserFromUsers { get; set; } = new List<MessageInboxUser>();

    public virtual ICollection<MessageInboxUser> MessageInboxUserToUsers { get; set; } = new List<MessageInboxUser>();

    public virtual ICollection<MessageOutboxUser> MessageOutboxUserFromUsers { get; set; } = new List<MessageOutboxUser>();

    public virtual ICollection<MessageOutboxUser> MessageOutboxUserToUsers { get; set; } = new List<MessageOutboxUser>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<PreOrder> PreOrders { get; set; } = new List<PreOrder>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
