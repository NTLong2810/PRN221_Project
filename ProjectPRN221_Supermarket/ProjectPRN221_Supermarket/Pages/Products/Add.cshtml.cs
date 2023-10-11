using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_Supermarket.Models;
using ProjectPRN221_Supermarket.Repository;

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
        public List<Category> Categories { get; set; }
        public void OnGet()
        {
            Categories = _context.Categories.ToList();
        }
        public IActionResult OnPost()
        {
                Product.ExpirationDate = ExpirationDate;
                _productRepository.AddProduct(Product);
            Categories = _context.Categories.ToList();
            return RedirectToPage("List");
       
        }
    }
}
