using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_Supermarket.Hubs;
using ProjectPRN221_Supermarket.Models;
using ProjectPRN221_Supermarket.Repository;

namespace ProjectPRN221_Supermarket.Pages.Products
{
	public class EditModel : PageModel
	{
		private readonly IProductRepository _productRepository;
		SupermarketDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<HubServer> _hubContext;
        public EditModel(IProductRepository productRepository, SupermarketDBContext context, IHttpContextAccessor httpContextAccessor, IHubContext<HubServer> hubContext)
		{
			_productRepository = productRepository;
			_context = context;
			_httpContextAccessor = httpContextAccessor;
			_hubContext = hubContext;
		}

		[BindProperty]
		public Product Product { get; set; }
		public List<Category> Categories { get; set; }

		public IActionResult OnGet(int id)
		{
            var cashierId = _httpContextAccessor.HttpContext.Session.GetString("CashierId");

            // Kiểm tra xem có thông tin người dùng trong phiên không
            if (string.IsNullOrEmpty(cashierId))
            {
                // Chuyển hướng đến trang đăng nhập nếu chưa đăng nhập
                return Redirect("/Login");
            }
            Product = _productRepository.GetProductById(id);
			if (Product == null)
			{
				return RedirectToPage("List");
			}
			Categories = _context.Categories.ToList();
			return Page();
		}

		public IActionResult OnPost()
		{

			if (!ModelState.IsValid)
			{
				// Load the original product and related purchase order item from the database
				var originalProduct = _context.Products
					.Include(p => p.PurchaseOrderItems)
					.FirstOrDefault(p => p.ProductId == Product.ProductId);

				if (originalProduct == null)
				{
					return RedirectToPage("List");
				}

				// Check and update UnitPrice
				if (Product.UnitPrice < originalProduct.PurchaseOrderItems.Sum(poi => poi.UnitPrice))
				{
					ModelState.AddModelError("Product.UnitPrice", "UnitPrice cannot be less than the total UnitPrice of related PurchaseOrderItems.");
					Categories = _context.Categories.ToList();
					return Page();
				}

				// Check and update QuantityInStock
				if (Product.QuantityInStock > originalProduct.PurchaseOrderItems.Sum(poi => poi.Quantity))
				{
					ModelState.AddModelError("Product.QuantityInStock", "QuantityInStock cannot be greater than the total Quantity of related PurchaseOrderItems.");
					Categories = _context.Categories.ToList();
					return Page();
				}

				// Update the product
				originalProduct.ProductName = Product.ProductName;
				originalProduct.UnitPrice = Product.UnitPrice;
				originalProduct.QuantityInStock = Product.QuantityInStock;
				originalProduct.CategoryId = Product.CategoryId;

				_context.SaveChanges();
                _hubContext.Clients.All.SendAsync("ReceiveChangeProduct");
                return RedirectToPage("List");
			}

			Categories = _context.Categories.ToList();
			return Page();
		}
	}
}
