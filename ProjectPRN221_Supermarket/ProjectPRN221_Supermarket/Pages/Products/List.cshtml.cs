using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_Supermarket.Models;
using ProjectPRN221_Supermarket.Pages.Paging;
using ProjectPRN221_Supermarket.Repository;

namespace ProjectPRN221_Supermarket.Pages.Products
{
    public class ListModel : PageModel
    {
        private SupermarketDBContext _context;
        private readonly IProductRepository _productRepository;
        public ListModel(SupermarketDBContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }
        [BindProperty]
        public PaginatedList<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        [BindProperty(SupportsGet = true)]
        public string productName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? categoryId { get; set; }
        public IList<Product> Product { get; set; } = default!;
        public List<Product> ExpiringProducts { get; set; }
        public async Task OnGet(int? pageIndex)
        {
            var pageSize = 4; 
            IQueryable<Product> products = _context.Products.Include(c => c.Category).AsNoTracking();
            products = products.OrderBy(p => p.ExpirationDate).ThenByDescending(p => p.QuantityInStock);
            Products = await PaginatedList<Product>.CreateAsync(
                products, pageIndex ?? 1, pageSize);
            Categories = _context.Categories.ToList();
        }
        public async Task OnPost(string productName, int? categoryId)
        {
            var query = _context.Products.Include(c => c.Category).AsQueryable();

            if (!string.IsNullOrEmpty(productName))
            {
                query = query.Where(p => p.ProductName.Contains(productName));
            }

            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            Products = await PaginatedList<Product>.CreateAsync(query, 1, 4);
            Categories = _context.Categories.ToList();
        }
    }
}
