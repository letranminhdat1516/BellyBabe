using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Model.Statistics
{
    public class MonthlyStatistics
    {
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public List<CategorySales> CategorySales { get; set; }
        public List<ProductSales> MostSoldProducts { get; set; }
    }
}
