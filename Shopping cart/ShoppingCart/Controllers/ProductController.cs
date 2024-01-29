using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace ShoppingCart.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AccountController> _logger;
        public ProductController(ILogger<AccountController> logger,IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [Route("/ProductList")]
        public IActionResult ProductList()
        {
            List<Product> products = ProductData.GetAllProducts();
            ViewBag.Products = products;
            ViewBag.Productsjson = JsonConvert.SerializeObject(products);
            
            return View();
        }

        public IActionResult AddToCart(int productId)
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            string? sessionId = httpContext.Session.GetString("sessionid");
            string? username = httpContext.Session.GetString("username");

			// Update the cart item in the database
			CalculateCount calResult = ProductData.AddItemToCart(sessionId, productId, username);

            // Return the updated total amount as JSON
            return Json(new { 
				status = calResult.status_code,
				total_quantity = calResult.total_quantity
			});
        }

		public IActionResult AddCount()
		{
			HttpContext? httpContext = _httpContextAccessor.HttpContext;
			string? sessionId = httpContext.Session.GetString("sessionid");
			string? username = httpContext.Session.GetString("username");

			// Update the cart item in the database
			int total_quantity = ProductData.CartCount(sessionId,username);

			// Return the updated total amount as JSON
			return Json(new { total_quantity = total_quantity });
		}
	}
}
