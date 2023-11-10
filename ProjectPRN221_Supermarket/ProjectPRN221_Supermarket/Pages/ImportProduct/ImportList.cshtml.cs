using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_Supermarket.Models;

namespace ProjectPRN221_Supermarket.Pages.ImportProduct
{
    public class ImportListModel : PageModel
    {
        private readonly SupermarketDBContext _context;
        public ImportListModel(SupermarketDBContext context)
        {
            _context = context;
        }
        public List<PurchaseOrder> PurchaseOrder { get; set; }
        public void OnGet()
        {
            PurchaseOrder = _context.PurchaseOrders
                                .Include(s => s.Supplier)
                                .Include(po => po.PurchaseOrderItems)
                                .ThenInclude(p => p.Product)
                                .ToList();
        }
    }
}
