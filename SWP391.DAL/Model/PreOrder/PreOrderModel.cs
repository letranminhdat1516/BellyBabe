using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Model.PreOrder
{
    public class PreOrderModel
    {
        public int PreOrderId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? PreOrderDate { get; set; }
        public bool? NotificationSent { get; set; }
    }
}
