using Microsoft.Data.SqlClient;
using ShoppingCart.Models;
using System.Data;

namespace ShoppingCart.Data
{
    public class ProductData : Data
    {

        public static List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            // Define your stored procedure name
            string storedProcedureName = "getAllProductsWithRatings";

            // Define your connection string
            string connectionString = @"Server=(local);Database=ShoppingCartDB;Integrated Security=true;encrypt=false";

            // Create a SqlConnection object using the connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a SqlCommand object using the stored procedure name and SqlConnection object
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    // Set the command type to stored procedure
                    command.CommandType = CommandType.StoredProcedure;

                    // Execute the stored procedure
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Product product = new Product()
                        {
                            Id = (int)reader["product_id"],
                            Name = (string)reader["product_name"],
                            Description = (string)reader["description"],
                            Price = (decimal)reader["price"],
                            ImageUrl = (string)reader["imagepath"],
                            Ratings = reader["average_rating"] == DBNull.Value ? null : (decimal)reader["average_rating"]
                        };
                        products.Add(product);
                    }

                    // Close the SqlDataReader object
                    reader.Close();
                }
            }

            return products;

        }

        public static CalculateCount AddItemToCart(string sessionid, int product_id, string username) //Stores in DB - for logged in users 
        {
            string storedProcedureName = "add_cart"; 
            int status_code=0;
            CalculateCount calResult=new CalculateCount();
			// Define your connection string
			string connectionString = Data.connectionString;

            // Create a SqlConnection object using the connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a SqlCommand object using the stored procedure name and SqlConnection object
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    // Set the command type to stored procedure
                    command.CommandType = CommandType.StoredProcedure;

                    // Add any input parameters to the stored procedure, if needed
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@product_id", product_id);
                    command.Parameters.AddWithValue("@sessionid", sessionid);

                    // Execute the stored procedure
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
						CalculateCount Result = new CalculateCount()
                        {
							status_code = (int)reader["status_code"],
                            total_quantity = reader["total_quantity"] == DBNull.Value ? 0 : (int)reader["total_quantity"]
						};
						calResult = Result;
					}

					// Close the SqlDataReader object
					reader.Close();
                }
            }
            return calResult;
        }

        public static int CartCount(string sessionid, string username) //Stores in DB - for logged in users 
        {
            string storedProcedureName = "get_cart_item_count"; 
            int total_quantity = 0;
            // Define your connection string
            string connectionString = Data.connectionString;

            // Create a SqlConnection object using the connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a SqlCommand object using the stored procedure name and SqlConnection object
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    // Set the command type to stored procedure
                    command.CommandType = CommandType.StoredProcedure;

                    // Add any input parameters to the stored procedure, if needed
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@sessionid", sessionid);

                    // Execute the stored procedure
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        total_quantity = reader["total_quantity"] == DBNull.Value ? 0: (int)reader["total_quantity"];
                    }
                    // Close the SqlDataReader object
                    reader.Close();
                }
            }
            return total_quantity;
        }
    }
}
