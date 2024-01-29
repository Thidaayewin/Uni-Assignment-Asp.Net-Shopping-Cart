namespace ShoppingCart.Models
{
    public class Cart
    {
        public int? cart_item_id { get; set; }
        public int? customer_id { get; set; }    
        public int? product_id { get; set; } 
        public int? quantity { get; set; }
        public string? sessionid { get; set; } 
        public string? product_name { get; set; }    
        public string? description { get; set; } 
        public string? imagepath { get; set; }
        public decimal? price { get; set; }
        public decimal? totalprice { get; set; }

	}
}
