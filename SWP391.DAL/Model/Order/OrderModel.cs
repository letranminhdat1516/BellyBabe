﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Model.Order
{
    public class OrderModel
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public string? Note { get; set; }

        public int? VoucherId { get; set; }

        public int? TotalPrice { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? RecipientName { get; set; }

        public string? RecipientPhone { get; set; }

        public string? RecipientAddress { get; set; }

    }
}
