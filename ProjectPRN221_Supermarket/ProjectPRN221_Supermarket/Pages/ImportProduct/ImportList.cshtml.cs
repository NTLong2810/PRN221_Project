using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_Supermarket.Models;

namespace ProjectPRN221_Supermarket.Pages.ImportProduct
{
    public class ImportListModel : PageModel
    {
        private readonly SupermarketDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ImportListModel(SupermarketDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public List<PurchaseOrder> PurchaseOrder { get; set; }
        public IActionResult OnGet()
        {
            var cashierId = _httpContextAccessor.HttpContext.Session.GetString("CashierId");

            // Kiểm tra xem có thông tin người dùng trong phiên không
            if (string.IsNullOrEmpty(cashierId))
            {
                // Chuyển hướng đến trang đăng nhập nếu chưa đăng nhập
                return Redirect("/Login");
            }
            PurchaseOrder = _context.PurchaseOrders
                                .Include(s => s.Supplier)
                                .Include(po => po.PurchaseOrderItems)
                                .ThenInclude(p => p.Product)
                                .ToList();
            return Page();
        }
    }
}
