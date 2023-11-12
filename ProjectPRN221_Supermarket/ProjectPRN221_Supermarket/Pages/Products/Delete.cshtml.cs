using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using ProjectPRN221_Supermarket.Hubs;
using ProjectPRN221_Supermarket.Models;
using ProjectPRN221_Supermarket.Repository;

namespace ProjectPRN221_Supermarket.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<HubServer> _hubContext;
        public DeleteModel(IProductRepository productRepository, IHttpContextAccessor httpContextAccessor, IHubContext<HubServer> hubContext)
        {
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Product Product { get; set; }

        public IActionResult OnGet(int id)
        {
            var cashierId = _httpContextAccessor.HttpContext.Session.GetString("CashierId");

            // Kiểm tra xem có thông tin người dùng trong phiên không
            if (string.IsNullOrEmpty(cashierId))
            {
                return Redirect("/Login");
            }
            Product = _productRepository.GetProductById(id);
            if (Product == null)
            {
                return RedirectToPage("List");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            _productRepository.DeleteProduct(Product.ProductId);
            _hubContext.Clients.All.SendAsync("ReceiveChangeProduct");
            return RedirectToPage("List");
        }
    }
}
