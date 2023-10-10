using System;
using System.Collections.Generic;

namespace ProjectPRN221_Supermarket.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            PurchaseOrders = new HashSet<PurchaseOrder>();
        }

        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    }
}
