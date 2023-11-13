using ProjectPRN221_Supermarket.Models;
using System.Text.Json;

namespace ProjectPRN221_Supermarket.Service
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddToCart(int cashierId, Product item)
        {
            var cart = GetCartFromCookie(cashierId);

            // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng hay chưa
            var existingItem = cart.FirstOrDefault(i => i.ProductItem.ProductId == item.ProductId);
            if (existingItem != null)
            {
                // Nếu sản phẩm đã tồn tại, tăng số lượng lên
                existingItem.Quantity += 1;
            }
            else
            {
                // Nếu sản phẩm chưa tồn tại, thêm vào giỏ hàng
                cart.Add(new CartItem { CashierId = cashierId, ProductItem = item, Quantity = 1 });
            }

            // Lưu giỏ hàng vào cookie
            SaveCartToCookie(cashierId, cart);
        }

        public List<CartItem> GetCart()
        {
            var cashierId = GetCurrentCashierId();
            return GetCartFromCookie(cashierId);
        }

        public void ClearCart()
        {
            var cashierId = GetCurrentCashierId();
            // Xóa giỏ hàng trong cookie
            _httpContextAccessor.HttpContext.Response.Cookies.Delete($"Cart_{cashierId}");
        }

        private List<CartItem> GetCartFromCookie(int cashierId)
        {
            var cartJson = _httpContextAccessor.HttpContext.Request.Cookies[$"Cart_{cashierId}"];
            if (!string.IsNullOrEmpty(cartJson))
            {
                // Chuyển đổi dữ liệu JSON trong cookie thành danh sách sản phẩm
                return JsonSerializer.Deserialize<List<CartItem>>(cartJson);
            }
            return new List<CartItem>();
        }

        private void SaveCartToCookie(int cashierId, List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);

            // Lưu giỏ hàng vào cookie
            _httpContextAccessor.HttpContext.Response.Cookies.Append($"Cart_{cashierId}", cartJson, new CookieOptions
            {
                // Đặt thời gian sống của cookie
                Expires = DateTimeOffset.Now.AddDays(7)
            });
        }

        private int GetCurrentCashierId()
        {
            var cashierIdString = _httpContextAccessor.HttpContext.Session.GetString("CashierId");
            return int.TryParse(cashierIdString, out int cashierId) ? cashierId : 0;
        }
    }
}
