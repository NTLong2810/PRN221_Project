using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_Supermarket.Models;

namespace ProjectPRN221_Supermarket.Pages.SellProduct
{
    public class SalesListModel : PageModel
    {
        private readonly SupermarketDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SalesListModel(SupermarketDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<SalesOrder> Sale { get; set; }

        public IActionResult OnGet()
        {
            var cashierId = _httpContextAccessor.HttpContext.Session.GetString("CashierId");

            // Kiểm tra xem có thông tin người dùng trong phiên không
            if (string.IsNullOrEmpty(cashierId))
            {
                // Chuyển hướng đến trang đăng nhập nếu chưa đăng nhập
                return Redirect("/Login");
            }
            Sale =  _context.SalesOrders
                .Include(o => o.SalesOrderItems)
                .ThenInclude(od => od.Product)
                .ToList();
            return Page();
        }
    }
}
