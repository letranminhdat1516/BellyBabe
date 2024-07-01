using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Model.Voucher
{
    public class SendVoucherRequest
    {
        public List<int> UserIds { get; set; }
        public string VoucherCode { get; set; }
    }
}
