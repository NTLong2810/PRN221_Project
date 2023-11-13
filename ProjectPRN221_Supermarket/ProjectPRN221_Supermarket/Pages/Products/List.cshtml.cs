using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_Supermarket.Hubs;
using ProjectPRN221_Supermarket.Models;
using ProjectPRN221_Supermarket.Pages.Paging;
using ProjectPRN221_Supermarket.Repository;
using ProjectPRN221_Supermarket.Service;

namespace ProjectPRN221_Supermarket.Pages.Products
{
    public class ListModel : PageModel
    {
        private readonly CartService _cartService;
        private SupermarketDBContext _context;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<HubServer> _hubContext;
        public ListModel(SupermarketDBContext context, IProductRepository productRepository, CartService cartService, IHttpContextAccessor httpContextAccessor, IHubContext<HubServer> hubContext)
        {
            _context = context;
            _productRepository = productRepository;
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
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
        public List<CartItem> CartItems { get; set; }
        public async Task<IActionResult> OnGet(int? pageIndex)
        {
            var cashierId = _httpContextAccessor.HttpContext.Session.GetString("CashierId");

            // Kiểm tra xem có thông tin người dùng trong phiên không
            if (string.IsNullOrEmpty(cashierId))
            {
                // Chuyển hướng đến trang đăng nhập nếu chưa đăng nhập
                return Redirect("/Login");
            }
            var pageSize = 4;
            IQueryable<Product> products = _context.Products.Include(c => c.Category).Include(p => p.PurchaseOrderItems).AsNoTracking();
            products = products.OrderBy(p => p.ExpirationDate).ThenByDescending(p => p.QuantityInStock);
            Products = await PaginatedList<Product>.CreateAsync(
                products, pageIndex ?? 1, pageSize);
            Categories = _context.Categories.ToList();
            CartItems = _cartService.GetCart();
            return Page();
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
            CartItems = _cartService.GetCart();
        }
        public IActionResult OnPostAddToCart(int productId, string productName, decimal price, int quantity)
        {
               var cashierId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("CashierId"));
            // Lấy thông tin sản phẩm từ cơ sở dữ liệu
            var product = _context.Products.Find(productId);
            CartItems = _cartService.GetCart();
            // Kiểm tra nếu sản phẩm không tồn tại
            if (product == null)
            {
                return NotFound();
            }

            // Thêm sản phẩm vào giỏ hàng
            var cartProduct = new Product
            {
                ProductId = productId,
                ProductName = productName,
                UnitPrice = price,
                QuantityInStock = quantity,
            };
            var totalQuantityInCart = CartItems
         .Where(item => item.ProductItem.ProductId == productId)
         .Sum(item => item.Quantity);

            if (totalQuantityInCart >= product.QuantityInStock)
            {
                TempData["OutOfStockMessage"] = $"Quantity exceeds the available stock for product '{productName}'.";
                _hubContext.Clients.All.SendAsync("ReceiveOutOfStockMessage", TempData["OutOfStockMessage"]);
                return RedirectToPage("/Products/List");
            }

            if (product.QuantityInStock <= 0)
            {
                TempData["OutOfStockMessage"] = $"Product '{cartProduct.ProductName}' is out of stock.";
                _hubContext.Clients.All.SendAsync("ReceiveOutOfStockMessage", TempData["OutOfStockMessage"]);
                return RedirectToPage("/Products/List");
            }

            _cartService.AddToCart(cashierId, cartProduct);

            // Chuyển hướng về trang giỏ hàng
            return RedirectToPage("/Products/List");
        }

        public IActionResult OnPostClearCart()
        {
            // Xóa giỏ hàng
            _cartService.ClearCart();

            // Chuyển hướng về trang chủ
            return RedirectToPage("/Products/List");
        }
        public IActionResult OnPostCreateOrder()
        {
            var cashierId = _httpContextAccessor.HttpContext.Session.GetString("CashierId");

            // Kiểm tra xem có thông tin người dùng trong phiên không
            if (string.IsNullOrEmpty(cashierId))
            {
                // Chuyển hướng đến trang đăng nhập nếu chưa đăng nhập
                return Redirect("/Login");
            }

            // Chuyển CashierId từ string sang int
            if (!int.TryParse(cashierId, out int cashierIdInt))
            {
                return BadRequest("Invalid CashierId");
            }

            CartItems = _cartService.GetCart();

            // Kiểm tra xem có sản phẩm trong giỏ hàng hết hàng không
            foreach (var cartItem in CartItems)
            {
                var product = _context.Products.Find(cartItem.ProductItem.ProductId);
                if (product != null && product.QuantityInStock < cartItem.Quantity)
                {
                    // Nếu có ít nhất một sản phẩm trong giỏ hàng hết hàng, thông báo và không tạo đơn hàng
                    TempData["OutOfStockMessage"] = $"Product '{cartItem.ProductItem.ProductName}' is out of stock.";
                    _hubContext.Clients.All.SendAsync("ReceiveOutOfStockMessage", TempData["OutOfStockMessage"]);
                    return RedirectToPage("/Products/List");
                }
            }
            var order = new SalesOrder
            {
                CashierId = cashierIdInt,
                OrderDate = DateTime.Now,
                TotalAmount = CalculateTotalAmount(CartItems)
            };

            _context.SalesOrders.Add(order);
            _context.SaveChanges();

            // Tạo chi tiết đơn hàng (OrderDetail) cho từng sản phẩm trong giỏ hàng
            foreach (var cartItem in CartItems)
            {
                // Giảm số lượng tồn kho chỉ nếu sản phẩm có sẵn
                var product = _context.Products.Find(cartItem.ProductItem.ProductId);
                if (product != null)
                {
                    product.QuantityInStock -= cartItem.Quantity;
                }

                var orderDetail = new SalesOrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = cartItem.ProductItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Quantity * cartItem.ProductItem.UnitPrice
                };
                _context.SalesOrderItems.Add(orderDetail);
            }

            _context.SaveChanges();
            // Xóa sản phẩm khỏi giỏ hàng sau khi tạo đơn hàng
            _cartService.ClearCart();
            _hubContext.Clients.All.SendAsync("ReceiveSalesUpdate");
            return RedirectToPage("/SellProduct/SalesList");
        }

        private decimal CalculateTotalAmount(List<CartItem> cartItems)
        {
            decimal totalAmount = 0;
            foreach (var item in cartItems)
            {
                totalAmount += item.Quantity * item.ProductItem.UnitPrice;
            }
            return totalAmount;
        }
    }
}

