using System;

namespace SWP391.DAL.Entities.Chat
{
    public class CustomerOption
    {
        public int CustomerOptionId { get; set; }
        public string UserName { get; set; }
        public string Option { get; set; }
        public DateTime DateSelected { get; set; }
    }
}
