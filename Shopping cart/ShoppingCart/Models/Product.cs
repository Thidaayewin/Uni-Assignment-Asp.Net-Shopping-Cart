
namespace ShoppingCart.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public decimal? Ratings { get; set; }
    }

    public class CalculateCount
    {
        public int status_code { get; set; }
        public int total_quantity { get; set; }
    }
}
