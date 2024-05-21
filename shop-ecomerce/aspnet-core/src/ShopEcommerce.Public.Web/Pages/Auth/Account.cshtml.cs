using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ShopEcommerce.Public.Web.Pages.Auth
{
    public class AccountModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public AccountModel(IConfiguration configuraiton)
        {
            _configuration = configuraiton;
        }
        public IActionResult OnGet()
        {
            return Redirect(_configuration["AuthServer:Authority"] + "/" + "Account/Manage");
        }
    }
}
