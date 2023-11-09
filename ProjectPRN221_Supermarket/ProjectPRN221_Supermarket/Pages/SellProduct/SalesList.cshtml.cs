using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_Supermarket.Models;

namespace ProjectPRN221_Supermarket.Pages.SellProduct
{
    public class SalesListModel : PageModel
    {
        private readonly SupermarketDBContext _context;

        public SalesListModel(SupermarketDBContext context)
        {
            _context = context;
        }

        public List<SalesOrder> Sale { get; set; }

        public void OnGet()
        {
            Sale =  _context.SalesOrders
                .Include(o => o.SalesOrderItems)
                .ThenInclude(od => od.Product)
                .ToList();
        }
    }
}
