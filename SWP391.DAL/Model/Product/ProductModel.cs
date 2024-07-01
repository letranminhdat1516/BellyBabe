using SWP391.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Model.Product
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public bool? IsSelling { get; set; }

        public string? Description { get; set; }

        public int Quantity { get; set; }

        public int IsSoldOut { get; set; }

        public DateTime? BackInStockDate { get; set; }

        public int? CategoryId { get; set; }

        public int? BrandId { get; set; }

        public int? FeedbackTotal { get; set; }

        public int? OldPrice { get; set; }

        public decimal? Discount { get; set; }

        public decimal? NewPrice { get; set; }

        public string? ImageLinks { get; set; }

        public virtual Brand? Brand { get; set; }

        public virtual ProductCategory? Category { get; set; }
    }
}
