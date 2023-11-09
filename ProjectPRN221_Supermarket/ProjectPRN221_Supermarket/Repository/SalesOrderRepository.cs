using ProjectPRN221_Supermarket.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjectPRN221_Supermarket.Repository
{
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private readonly SupermarketDBContext _context;

        public SalesOrderRepository(SupermarketDBContext context)
        {
            _context = context;
        }

        public List<SalesOrderItem> GetSalesOrderItems()
        {
            // Lấy danh sách SalesOrderItems từ cơ sở dữ liệu
            return _context.SalesOrderItems
                .Include(soi => soi.Order)
                .Include(soi => soi.Product)
                .ToList();
        }

        public void SellProduct(int productId, int quantity)
        {
            // Giả sử bạn đã có phương thức giảm số lượng trong cơ sở dữ liệu tại đây
            var product = _context.Products.Find(productId);
            if (product != null && product.QuantityInStock >= quantity)
            {
                // Giảm số lượng trong kho
                product.QuantityInStock -= quantity;

                // Tạo một đơn hàng mới
                var salesOrder = new SalesOrder
                {
                    OrderDate = DateTime.Now
                };
                _context.SalesOrders.Add(salesOrder);
                _context.SaveChanges();

                // Thêm một mục đơn hàng
                var salesOrderItem = new SalesOrderItem
                {
                    OrderId = salesOrder.OrderId,
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.UnitPrice
                };
                _context.SalesOrderItems.Add(salesOrderItem);
                _context.SaveChanges();
            }
        }
    }
}
