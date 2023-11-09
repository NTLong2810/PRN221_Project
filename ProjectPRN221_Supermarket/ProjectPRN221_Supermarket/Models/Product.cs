using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectPRN221_Supermarket.Models
{
    public partial class Product
    {
        public Product()
        {
            PurchaseOrderItems = new HashSet<PurchaseOrderItem>();
            SalesOrderItems = new HashSet<SalesOrderItem>();
        }

        public int ProductId { get; set; }
        [Required(ErrorMessage = "Product name is required!")]
        public string ProductName { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Unit Price must be a non-negative value.")]
        public decimal UnitPrice { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Quantity in Stock must be a non-negative integer.")]
        public int QuantityInStock { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ExpirationDate { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public virtual ICollection<SalesOrderItem> SalesOrderItems { get; set; }
    }
}
