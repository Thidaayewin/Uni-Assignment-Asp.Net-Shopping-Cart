using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using ShoppingCart.Models;
using System.Data;
using System.Reflection.PortableExecutable;

namespace ShoppingCart.Data
{
    public class CartData: Data
    {
        public CartData() { }
        public static List<Cart> GetAllCartData(string username,  string sessionId)
        {
            List<Cart> cartList = new List<Cart>();
            // Define your stored procedure name
            string storedProcedureName = "select_all_cart_products";

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

                    // Add any input parameters to the stored procedure, if needed
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@session_id", sessionId);

                    // Execute the stored procedure
                    SqlDataReader reader = command.ExecuteReader();
					while (reader.Read())
						{
							Cart cart = new Cart()
								{
									cart_item_id = (int)reader["cart_item_id"],
									customer_id = reader["customer_id"] == DBNull.Value ? null : (int)reader["customer_id"],
									product_id = (int)reader["product_id"],
									quantity = (int)reader["quantity"],
									sessionid = reader["sessionid"] == DBNull.Value ? null : (string)reader["sessionid"],
									product_name = (string)reader["product_name"],
									description = (string)reader["description"],
									imagepath = (string)reader["imagepath"],
									price = (decimal)reader["price"],
									totalprice = (decimal)reader["price"]
								};
								cartList.Add(cart);
							}
                    // Close the SqlDataReader object
                    reader.Close();
                }
            }
            return cartList;
        }

		

		        public static decimal UpdateItemToCartonLoadData(int index, int quantity, decimal total, int product_item_id, int customer_id, string sessionid)

		{
			decimal totalAmount = 0;
			string storedProcedureName = "update_shopping_cart_item";

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
                    command.Parameters.AddWithValue("@product_item_id", product_item_id);
                    command.Parameters.AddWithValue("@customer_id", customer_id);
                    command.Parameters.AddWithValue("@sessionid", sessionid);
                    command.Parameters.AddWithValue("@quantity", quantity);

					// Execute the stored procedure
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
                        totalAmount = reader["totalAmount"]==DBNull.Value?0:(decimal)reader["totalAmount"];
                    }
					// Close the SqlDataReader object
					reader.Close();
				}
			}
			return totalAmount;
		}

		public static int CheckoutOrder( string username,string sessionid)
		{
			int status_code = 0;
			// Define your stored procedure name
			string storedProcedureName = "confirm_order";

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
					
					command.Parameters.AddWithValue("@username", username);
					command.Parameters.AddWithValue("@sessionid", sessionid);

					// Execute the stored procedure
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						status_code = (int)reader["status_code"];
					}
					// Close the SqlDataReader object
					reader.Close();
				}
			}
			return status_code;
		}


		//cleanup SQL cart table associated with previous sessionID but no customerID, after a session has timed out
		public static void CleanupCart (string sessionid)
		{
            string storedProcedureName = "clean_cart";
            string connectionString = Data.connectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a SqlCommand object using the stored procedure name and SqlConnection object
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    // Set the command type to stored procedure
                    command.CommandType = CommandType.StoredProcedure;

                    // input parameters to the stored procedure
                    command.Parameters.AddWithValue("@sessionid", sessionid);

                    // Execute the stored procedure
                    SqlDataReader reader = command.ExecuteReader();

                    // Close the SqlDataReader object
                    reader.Close();
                }
            }

        }

	}
}
