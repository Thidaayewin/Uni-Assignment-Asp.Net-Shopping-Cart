using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

		public CartController(ILogger<CartController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("/CartList")]
        public IActionResult CartList()
        {
			HttpContext? httpContext = _httpContextAccessor.HttpContext;
			string? username = httpContext.Session.GetString("username");
			string? sessionid = httpContext.Session.GetString("sessionid");
			List<Cart> carts = CartData.GetAllCartData(username, sessionid);
            ViewBag.Carts = carts;
            return View();
        }

		[HttpPost]
		public IActionResult onLoadData(int index, int quantity, decimal total,int product_item_id,int customer_id)
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            string? username = httpContext.Session.GetString("username");
            string? sessionid = httpContext.Session.GetString("sessionid");
            decimal totalCost=CartData.UpdateItemToCartonLoadData(index, quantity, total, product_item_id, customer_id, sessionid);
			return Json(new { status = 200, totalCost= totalCost });
		}

		public IActionResult checkOutOrder()
		{
			HttpContext? httpContext = _httpContextAccessor.HttpContext;
			string? username=httpContext.Session.GetString("username");
			string? sessionid = httpContext.Session.GetString("sessionid");
			int status_code = CartData.CheckoutOrder(username, sessionid);
			return Json(new { status = status_code });

		}
	}
}
