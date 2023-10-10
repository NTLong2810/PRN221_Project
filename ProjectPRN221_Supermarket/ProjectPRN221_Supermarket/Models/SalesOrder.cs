using System;
using System.Collections.Generic;

namespace ProjectPRN221_Supermarket.Models
{
    public partial class SalesOrder
    {
        public SalesOrder()
        {
            SalesOrderItems = new HashSet<SalesOrderItem>();
        }

        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }

        public virtual ICollection<SalesOrderItem> SalesOrderItems { get; set; }
    }
}
