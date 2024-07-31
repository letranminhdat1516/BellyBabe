using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Entities
{
    public class OrderStatusHistory
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public int StatusId { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Note { get; set; }

        public virtual Order Order { get; set; } = null!;

        public virtual OrderStatus Status { get; set; } = null!;

        public virtual User User { get; set; } = null!;

    }
}
