namespace ShoppingCart.Models
{
    public class Customer
    {
        public int customerID { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? password { get; set; }
        public string? sessionID { get; set; }
    }
    public class LoginResult
    {
        public int status_code { get; set; }
    }
}
