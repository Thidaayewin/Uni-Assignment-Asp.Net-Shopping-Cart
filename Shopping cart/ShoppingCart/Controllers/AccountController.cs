using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private AccountData _accountData;
        public AccountController(ILogger<AccountController> logger, AccountData accountData, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _accountData = accountData;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Login()
        {
			HttpContext? httpContext = _httpContextAccessor.HttpContext;
			string? sessionId = httpContext.Session.GetString("sessionid");
			return View();
        }

        [HttpPost]
        public IActionResult onLogin(string username, string password)
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            string? sessionId = httpContext.Session.GetString("sessionid");
            // test for authentication
            LoginResult loginResult = _accountData.CheckCredentail(username, password, sessionId);
            if(loginResult.status_code == 200) //authentication successful
            {
                
                httpContext.Session.SetString("username", username);

                //retrieve basic information of customer from DB
                Customer customer = _accountData.RetrieveCustomerNameID(username);
                httpContext.Session.SetInt32("customer_id", customer.customerID);
                httpContext.Session.SetString("first_name", customer.firstName);
                httpContext.Session.SetString("last_name", customer.lastName);

            }
            httpContext.Session.SetString("sessionid", sessionId);
            return Json(new { status = loginResult.status_code });
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            string? sessionId = httpContext.Session.GetString("sessionid");
            if (sessionId == null)
            {
                sessionId = Guid.NewGuid().ToString();
            }
            httpContext.Session.SetString("sessionid", sessionId);
            return RedirectToAction("ProductList", "Product");
        }

       
            
        
    }
}
