using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_Supermarket.Models;
using ProjectPRN221_Supermarket.Repository;

namespace ProjectPRN221_Supermarket.Pages.SellProduct
{
    public class SalesModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;

        public SalesModel(IProductRepository productRepository, ISalesOrderRepository salesOrderRepository)
        {
            _productRepository = productRepository;
            _salesOrderRepository = salesOrderRepository;
        }

        [BindProperty]
        public int ProductId { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public List<Product> Products { get; set; }
        public List<SalesOrderItem> SalesOrderItems { get; set; }

        public void OnGet()
        {
            Products = (List<Product>)_productRepository.GetAllProducts();
            SalesOrderItems = _salesOrderRepository.GetSalesOrderItems();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _salesOrderRepository.SellProduct(ProductId, Quantity);
                return RedirectToPage();
            }

            Products = (List<Product>)_productRepository.GetAllProducts();
            SalesOrderItems = _salesOrderRepository.GetSalesOrderItems();
            return Page();
        }
    }
}
