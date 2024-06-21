using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Entities.Chat
{
    public class ChatMessageModel
    {
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
    }


}
