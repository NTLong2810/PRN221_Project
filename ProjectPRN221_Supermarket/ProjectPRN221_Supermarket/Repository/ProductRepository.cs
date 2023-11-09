using Microsoft.EntityFrameworkCore;
using ProjectPRN221_Supermarket.Models;

namespace ProjectPRN221_Supermarket.Repository
{
    public class ProductRepository:IProductRepository
    {
        private readonly SupermarketDBContext _context;

        public ProductRepository(SupermarketDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.Include(p => p.Category).ToList();
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public Product GetProductById(int productId)
        {
            return _context.Products.Include(p => p.Category)
                                   .FirstOrDefault(p => p.ProductId == productId);
        }
        public void DeleteProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
        public IEnumerable<Product> GetExpiringProducts(int daysBeforeExpiration)
        {
            DateTime expirationDateThreshold = DateTime.Now.AddDays(daysBeforeExpiration);
            return _context.Products
                .Where(p => p.ExpirationDate != null && p.ExpirationDate <= expirationDateThreshold)
                .ToList();
        }

    }
}
