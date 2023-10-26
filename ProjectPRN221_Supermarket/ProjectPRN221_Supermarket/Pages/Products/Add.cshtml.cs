using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_Supermarket.Models;
using ProjectPRN221_Supermarket.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPRN221_Supermarket.Pages.Products
{
    public class AddModel : PageModel
    {
        IProductRepository _productRepository;
        SupermarketDBContext _context;

        public AddModel(IProductRepository productRepository, SupermarketDBContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public DateTime ExpirationDate { get; set; }

        [BindProperty]
        public int SelectedSupplier { get; set; }

        [BindProperty]
        public decimal PurchaseOrderUnitPrice { get; set; }

        public List<Category> Categories { get; set; }
        public List<Supplier> Suppliers { get; set; }

        public void OnGet()
        {
            Categories = _context.Categories.ToList();
            Suppliers = _context.Suppliers.ToList();
        }

        public IActionResult OnPost()
        {
            Categories = _context.Categories.ToList();
            Suppliers = _context.Suppliers.ToList();
            // Create the product
            Product.ExpirationDate = ExpirationDate;
            _productRepository.AddProduct(Product);

            // Create the purchase order
            PurchaseOrder purchaseOrder = new PurchaseOrder
            {
                SupplierId = SelectedSupplier,
                OrderDate = DateTime.Now, // You may want to set the order date accordingly.
            };
            _context.PurchaseOrders.Add(purchaseOrder);
            _context.SaveChanges();

            // Create the purchase order item
            PurchaseOrderItem purchaseOrderItem = new PurchaseOrderItem
            {
                OrderId = purchaseOrder.OrderId,
                ProductId = Product.ProductId,
                Quantity = Product.QuantityInStock, // Assuming you want to use the product's quantity.
                UnitPrice = PurchaseOrderUnitPrice,
            };
            _context.PurchaseOrderItems.Add(purchaseOrderItem);

            // Save changes to the database
            _context.SaveChanges();

            return RedirectToPage("List");
        }
    }
}
