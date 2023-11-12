using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using ProjectPRN221_Supermarket.Hubs;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<HubServer> _hubContext;

        public AddModel(IProductRepository productRepository, SupermarketDBContext context, IHttpContextAccessor httpContextAccessor, IHubContext<HubServer> hubContext)
        {
            _productRepository = productRepository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
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

        public IActionResult OnGet()
        {
            var cashierId = _httpContextAccessor.HttpContext.Session.GetString("CashierId");

            // Kiểm tra xem có thông tin người dùng trong phiên không
            if (string.IsNullOrEmpty(cashierId))
            {
                // Chuyển hướng đến trang đăng nhập nếu chưa đăng nhập
                return Redirect("/Login");
            }
            Categories = _context.Categories.ToList();
            Suppliers = _context.Suppliers.ToList(); 
            return Page();
        }

        public IActionResult OnPost()
        {
            Categories = _context.Categories.ToList();
            Suppliers = _context.Suppliers.ToList();
            // Create the product
            int randomMonths = new Random().Next(3, 6);
            ExpirationDate = DateTime.Now.AddMonths(randomMonths);

            // Tạo sản phẩm
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
            _hubContext.Clients.All.SendAsync("ReceiveChangeProduct");
            return RedirectToPage("List");
        }
    }
}
