using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Model.Statistics
{
    public class ProductSales
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalSold { get; set; }
        public decimal TotalSales { get; set; }
    }
}
