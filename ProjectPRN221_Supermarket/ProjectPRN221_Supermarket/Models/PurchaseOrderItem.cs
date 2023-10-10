using System;
using System.Collections.Generic;

namespace ProjectPRN221_Supermarket.Models
{
    public partial class PurchaseOrderItem
    {
        public int OrderItemId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }

        public virtual PurchaseOrder Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
