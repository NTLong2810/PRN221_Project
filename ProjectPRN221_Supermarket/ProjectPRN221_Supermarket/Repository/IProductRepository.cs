using ProjectPRN221_Supermarket.Models;

namespace ProjectPRN221_Supermarket.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        void AddProduct(Product product);
        public Product GetProductById(int productId);
        public void DeleteProduct(int productId);
        public void UpdateProduct(Product product);
    }
}
