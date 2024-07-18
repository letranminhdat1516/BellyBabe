using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Model.PreOrder
{
    public class NotifyCustomerModel
    {
        public int PreOrderId { get; set; }
        public string Email { get; set; }
        public string ProductName { get; set; }
    }
}
