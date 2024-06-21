using System;
using System.Collections.Generic;
using WebApplication1.Entities;

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

    public string? OTP { get; set; }

    public DateTime? OTPExpiry { get; set; }
    public virtual ICollection<Message> Messages { get; set; }
    public virtual ICollection<MessageInboxUser> MessageInboxUserFromUserNameNavigations { get; set; }
    public virtual ICollection<MessageInboxUser> MessageInboxUserToUserNameNavigations { get; set; }
    public virtual ICollection<MessageOutboxUser> MessageOutboxUserFromUserNameNavigations { get; set; }
    public virtual ICollection<MessageOutboxUser> MessageOutboxUserToUserNameNavigations { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<CumulativeScoreTransaction> CumulativeScoreTransactions { get; set; } = new List<CumulativeScoreTransaction>();

    public virtual ICollection<CumulativeScore> CumulativeScores { get; set; } = new List<CumulativeScore>();

    public virtual ICollection<FeedbackResponse> FeedbackResponses { get; set; } = new List<FeedbackResponse>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<PreOrder> PreOrders { get; set; } = new List<PreOrder>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
