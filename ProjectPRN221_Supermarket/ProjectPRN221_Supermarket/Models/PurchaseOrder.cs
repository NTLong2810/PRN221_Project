using System;
using System.Collections.Generic;

namespace ProjectPRN221_Supermarket.Models
{
    public partial class PurchaseOrder
    {
        public PurchaseOrder()
        {
            PurchaseOrderItems = new HashSet<PurchaseOrderItem>();
        }

        public int OrderId { get; set; }
        public int? SupplierId { get; set; }
        public DateTime? OrderDate { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
    }
}
