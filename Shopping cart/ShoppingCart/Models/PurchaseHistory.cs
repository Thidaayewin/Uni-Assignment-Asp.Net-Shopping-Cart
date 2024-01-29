namespace ShoppingCart.Models
{
    public class PurchaseHistory
    {
        
        public string Name { get; set; }
        public string Description { get; set; }
        public string PurchaseDate { get; set; }
        public string ImageUrl { get; set; }
        public string ActivationCode { get; set; }
        public int Review { get; set; }
        public int Quantity { get; set; }
		public decimal Price { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
    }
}
