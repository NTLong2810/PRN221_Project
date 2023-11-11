using ProjectPRN221_Supermarket.Models;

namespace ProjectPRN221_Supermarket.Service
{
    public class AuthService
    {
        private readonly SupermarketDBContext _context;

        public AuthService(SupermarketDBContext context)
        {
            _context = context;
        }

        public CashierLogin Authenticate(string username, string password)
        {
            return _context.CashierLogins.SingleOrDefault(c => c.Username == username && c.Password == password);
        }
    }
}
