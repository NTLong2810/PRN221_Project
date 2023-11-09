using ProjectPRN221_Supermarket.Models;

namespace ProjectPRN221_Supermarket.Repository
{
    public interface ISalesOrderRepository
    {
        List<SalesOrderItem> GetSalesOrderItems();
        void SellProduct(int productId, int quantity);
    }
}
