using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Model.Chat
{
    public class ChatMessageModel
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public bool IsAdmin { get; set; }
    }
}

