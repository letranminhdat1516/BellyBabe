using SWP391.DAL.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Model.Order
{
    public class OrderDetailModel
    {
        public int? ProductId { get; set; }

        public int? Price { get; set; }

        public int? Quantity { get; set; }
        public ProductModel Product { get; set; }

    }
}
