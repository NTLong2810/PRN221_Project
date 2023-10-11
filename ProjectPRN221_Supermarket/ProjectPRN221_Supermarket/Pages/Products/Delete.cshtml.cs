using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_Supermarket.Models;
using ProjectPRN221_Supermarket.Repository;

namespace ProjectPRN221_Supermarket.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IProductRepository _productRepository;

        public DeleteModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [BindProperty]
        public Product Product { get; set; }

        public IActionResult OnGet(int id)
        {
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
