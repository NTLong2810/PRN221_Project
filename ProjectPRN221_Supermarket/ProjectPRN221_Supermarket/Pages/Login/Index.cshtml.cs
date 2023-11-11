using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_Supermarket.Models;
using ProjectPRN221_Supermarket.Service;

namespace ProjectPRN221_Supermarket.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly AuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [BindProperty]
        public CashierLogin Credentials { get; set; }

        public IndexModel(AuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {

                var cashier = _authService.Authenticate(Credentials.Username, Credentials.Password);

                if (cashier != null)
                {
                    // Lưu thông tin người dùng vào phiên làm việc
                    _httpContextAccessor.HttpContext.Session.SetString("CashierId", cashier.Id.ToString());

                    return Redirect("/Index");
                }
                else
                {
                    if (!ModelState.IsValid) { 
                    ModelState.AddModelError(string.Empty, "Invalid username or password");
                    return Page();
                }
            }
            return Page();
        }
    }
}
