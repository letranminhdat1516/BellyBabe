using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Model.Chat
{
    public class CustomerOptionModel
    {
        public int? UserId { get; set; }
        public int? MessageId { get; set; }
        public int? InboxId { get; set; }
        public int? OutboxId { get; set; }
        public string OptionValue { get; set; }
    }
}
