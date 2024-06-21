using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Entities
{
    public class MessageOutboxUser
    {
        public int OutboxId { get; set; }
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
