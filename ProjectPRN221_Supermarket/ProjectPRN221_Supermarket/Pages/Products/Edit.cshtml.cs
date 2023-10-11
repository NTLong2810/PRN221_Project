using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_Supermarket.Models;
using ProjectPRN221_Supermarket.Repository;

namespace ProjectPRN221_Supermarket.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        SupermarketDBContext _context;
        public EditModel(IProductRepository productRepository, SupermarketDBContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }

        public IActionResult OnGet(int id)
        {
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

             _productRepository.UpdateProduct(Product);
            Categories = _context.Categories.ToList();
            return RedirectToPage("List");
        }
    }
}
