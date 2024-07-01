using System;

namespace SWP391.DAL.Entities
{
    public class Statistic
    {
        public int StatisticsId { get; set; }

        public string Date { get; set; } = null!;

        public int? NumberOfOrders { get; set; }

        public int? ItemsSold { get; set; }

        public decimal? TotalAmount { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? ProductId { get; set; }

        public int? OrderId { get; set; }

        public virtual Order? Order { get; set; }

        public virtual Product? Product { get; set; }

        public virtual ProductCategory? ProductCategory { get; set; }
    }
}
