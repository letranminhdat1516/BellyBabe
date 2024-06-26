using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Model.Voucher
{
    public class VoucherDTO
    {
        public string VoucherName { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int? UserId { get; set; }
    }

}
