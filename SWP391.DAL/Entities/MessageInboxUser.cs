using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities {
    public class MessageInboxUser
    {
        public int InboxId { get; set; }
        public string? FromUserName { get; set; }
        public string? ToUserName { get; set; }
        public int? MessageId { get; set; }
        public bool? IsView { get; set; }
        public DateTime? DateCreated { get; set; }

        public virtual User? FromUserNameNavigation { get; set; }
        public virtual User? ToUserNameNavigation { get; set; }
        public virtual Message? Message { get; set; }
    }

}


