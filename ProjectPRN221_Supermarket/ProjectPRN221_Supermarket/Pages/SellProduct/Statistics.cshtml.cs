using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_Supermarket.Models;

namespace ProjectPRN221_Supermarket.Pages.SellProduct
{
    public class StatisticsModel : PageModel
    {
        private readonly SupermarketDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StatisticsModel(SupermarketDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public decimal TotalRevenue { get; set; }
        public decimal TotalProfit { get; set; }
        public List<Product> ExpiringProducts { get; set; }
        public List<Product> BestSellingProducts { get; set; }
        public List<Product> NotSellingProducts { get; set; }

        public IActionResult OnGet()
        {
            var cashierId = _httpContextAccessor.HttpContext.Session.GetString("CashierId");

            // Kiểm tra xem có thông tin người dùng trong phiên không
            if (string.IsNullOrEmpty(cashierId))
            {
                // Chuyển hướng đến trang đăng nhập nếu chưa đăng nhập
                return Redirect("/Login");
            }
            // Tính doanh thu và lợi nhuận
            CalculateRevenueAndProfit();

            // Lấy danh sách sản phẩm sắp hết
            GetExpiringProducts();

            // Lấy danh sách sản phẩm bán chạy
            GetBestSellingProducts();

            // Lấy danh sách sản phẩm không bán được
            GetNotSellingProducts();
            return Page();
        }

        private void CalculateRevenueAndProfit()
        {
            // Tính doanh thu
            TotalRevenue = _context.SalesOrders
                 .Where(o => o.TotalAmount.HasValue) // Lọc bỏ các đơn hàng có TotalAmount là null
                 .Sum(o => o.TotalAmount ?? 0);
            TotalProfit = CalculateTotalCost();
        }
        private decimal CalculateTotalCost()
        {
            // Lấy danh sách sản phẩm bán được cùng với giá nhập ban đầu của từng sản phẩm
            var soldProducts = _context.SalesOrderItems
     .Include(oi => oi.Product)
     .Where(oi => oi.Quantity > 0) // Chỉ xem xét sản phẩm với số lượng bán dương
     .ToList();

            decimal totalProfit = (decimal)soldProducts.Sum(oi =>
            {
                // Lợi nhuận từng sản phẩm
                decimal productCost = oi.Product.UnitPrice - GetPurchaseOrderItemUnitPrice(oi.Product.ProductId);
                return oi.Quantity * productCost;
            });

            return totalProfit;
        }
        private decimal GetPurchaseOrderItemUnitPrice(int productId)
        {
            var purchaseOrderItem = _context.PurchaseOrderItems
                .Where(poi => poi.ProductId == productId)
                .OrderByDescending(poi => poi.Order.OrderDate)
                .FirstOrDefault();

            return purchaseOrderItem?.UnitPrice ?? 0;
        }
        private void GetExpiringProducts()
        {
            // Lấy danh sách sản phẩm sắp hết (ví dụ: sắp hết là khi còn dưới 10 sản phẩm)
            ExpiringProducts = _context.Products
                .Where(p => p.QuantityInStock < 30)
                .OrderBy(p => p.QuantityInStock)
                .ToList();
        }

        private void GetBestSellingProducts()
        {
            // Lấy danh sách sản phẩm bán chạy (ví dụ: 5 sản phẩm có doanh số bán nhiều nhất)
            BestSellingProducts = _context.Products
        .Include(p => p.SalesOrderItems) // Thêm Include để đảm bảo SalesOrderItems được load
        .OrderByDescending(p => p.SalesOrderItems.Sum(oi => oi.Quantity))
        .Take(5)
        .ToList();
        }

        private void GetNotSellingProducts()
        {
            // Lấy danh sách sản phẩm không bán được (ví dụ: các sản phẩm có số lượng bán là 0)
            NotSellingProducts = _context.Products
            .Include(p => p.SalesOrderItems) // Thêm Include để đảm bảo SalesOrderItems được load
            .Where(p => p.SalesOrderItems.Sum(oi => oi.Quantity) == 0)
            .ToList();
        }
    }
}
