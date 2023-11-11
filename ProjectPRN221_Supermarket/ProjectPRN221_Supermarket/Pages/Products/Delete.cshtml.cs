using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_Supermarket.Models;
using ProjectPRN221_Supermarket.Repository;

namespace ProjectPRN221_Supermarket.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DeleteModel(IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
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
            return RedirectToPage("List");
        }
    }
}
