using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ShoppingCart.Data;
using ShoppingCart.Models;
using Microsoft.AspNetCore.Mvc.Core;


namespace ShoppingCart.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PurchaseController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = new HttpContextAccessor();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PurchaseHistory()
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            string username= httpContext.Session.GetString("username");
            List<PurchaseHistory> purchaseHistories= PurchaseHistoryData.PurchaseHistory(username);
            ViewBag.PurchaseHistory = purchaseHistories;
            return View();
        }

		[HttpPost]
		public IActionResult SavingReview(int customerid,int productid,int rating)
        {
            Reviews review = new Reviews
            {
                customerid = customerid,
                productid = productid,
                rating = rating
            };
            bool success= PurchaseHistoryData.SaveReview(review);
			return Json(new { success = true });
		}

       



	}


}
