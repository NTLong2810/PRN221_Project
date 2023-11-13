namespace ProjectPRN221_Supermarket.Models
{
    public class CartItem
    {
        public int Quantity { get; set; }
        public int CashierId { get; set; }
        public Product ProductItem { get; set; }
        public CartItem()
        {

        }
    }
}
