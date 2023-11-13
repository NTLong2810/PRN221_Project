using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_Supermarket.Models;
using ProjectPRN221_Supermarket.Pages.Paging;

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

        public PaginatedList<SalesOrder> Sales { get; set; }
        public IList<SalesOrder> Sale { get; set; } = default!;

        public async Task<IActionResult> OnGet(int? pageIndex)
        {
            var cashierId = _httpContextAccessor.HttpContext.Session.GetString("CashierId");

            // Kiểm tra xem có thông tin người dùng trong phiên không
            if (string.IsNullOrEmpty(cashierId))
            {
                return Redirect("/Login");
            }
            var pageSize = 4; // Số lượng mục trên mỗi trang
            IQueryable<SalesOrder> saleQuery = _context.SalesOrders
                .Include(o => o.SalesOrderItems)
                .ThenInclude(od => od.Product)
                .Include(o => o.Cashier)
                .OrderByDescending(o => o.OrderDate).AsNoTracking();

            Sales = await PaginatedList<SalesOrder>.CreateAsync(
                saleQuery, pageIndex ?? 1, pageSize);

            return Page();
        }
    }
}
