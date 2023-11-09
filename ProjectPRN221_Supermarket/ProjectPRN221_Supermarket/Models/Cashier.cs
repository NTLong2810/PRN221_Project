using System;
using System.Collections.Generic;

namespace ProjectPRN221_Supermarket.Models
{
    public partial class Cashier
    {
        public Cashier()
        {
            SalesOrders = new HashSet<SalesOrder>();
        }

        public int CashierId { get; set; }
        public string CashierName { get; set; }

        public virtual CashierLogin CashierNavigation { get; set; }
        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
    }
}
