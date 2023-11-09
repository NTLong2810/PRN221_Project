using ProjectPRN221_Supermarket.Models;
using System.Text.Json;

namespace ProjectPRN221_Supermarket.Service
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _cartCookieName = "Cart";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddToCart(Product item)
        {
            var cart = GetCartFromCookie();

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
                cart.Add(new CartItem { ProductItem = item, Quantity = 1 });
            }

            // Lưu giỏ hàng vào cookie
            SaveCartToCookie(cart);
        }

        public List<CartItem> GetCart()
        {
            return GetCartFromCookie();
        }

        public void ClearCart()
        {
            // Xóa giỏ hàng trong cookie
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(_cartCookieName);
        }

        private List<CartItem> GetCartFromCookie()
        {
            var cartJson = _httpContextAccessor.HttpContext.Request.Cookies[_cartCookieName];
            if (!string.IsNullOrEmpty(cartJson))
            {
                // Chuyển đổi dữ liệu JSON trong cookie thành danh sách sản phẩm
                return JsonSerializer.Deserialize<List<CartItem>>(cartJson);
            }
            return new List<CartItem>();
        }

        private void SaveCartToCookie(List<CartItem> cart)
        {
            //var cartJson = JsonConvert.SerializeObject(cart);
            var cartJson = JsonSerializer.Serialize(cart);

            // Lưu giỏ hàng vào cookie
            _httpContextAccessor.HttpContext.Response.Cookies.Append(_cartCookieName, cartJson, new CookieOptions
            {
                // Đặt thời gian sống của cookie
                Expires = DateTimeOffset.Now.AddDays(7)
            });
        }
    }
}
