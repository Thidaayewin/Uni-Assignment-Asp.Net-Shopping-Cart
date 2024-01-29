
using System.Collections.Generic;
using ShoppingCart.Models;
using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.Data
{
    public class PurchaseHistoryData : Data
    {
        public PurchaseHistoryData() { }
        public static List<PurchaseHistory> PurchaseHistory(string username)
        {
            //new list object for purchase history
            List<PurchaseHistory> purchasehistory = new List<PurchaseHistory>();

            //new stored procedure
            string storedProcedureName = "select_all_purchase_history";

            //New connection
            string connectionString = @"Server=(local);Database=ShoppingCartDB;Integrated Security=true;encrypt=false";

            //creating sql connection object
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                //setting command to stored procedure
                using (SqlCommand command = new SqlCommand(storedProcedureName, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@email", username);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        PurchaseHistory purchased = new PurchaseHistory()
                        {
                            //all attrbitues imagepath=url etcetc
                            Description = (string)reader["description"],
                            Name = (string)reader["product_name"],
                            PurchaseDate = (string)reader["created_date"],
                            ImageUrl = (string)reader["imagepath"],
                            ActivationCode = reader["activationCode"]==DBNull.Value? "": (string)reader["activationCode"],
                            Review = reader["rating"] == DBNull.Value ? 0 : (Int32)reader["rating"],
                            Quantity = (int)reader["quantity"],
                            ProductId = (int)reader["product_id"],
                            CustomerId = (int)reader["customer_id"]
                        };

                        purchasehistory.Add(purchased);
                    }

                }
                conn.Close();
            }
            return purchasehistory;
        }


		public static bool SaveReview(Reviews review)
		{
			string storedProcedureName = "save_reviews"; 
			int status_code = 0;
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
					command.Parameters.AddWithValue("@review_id", review.reviewid);
					command.Parameters.AddWithValue("@customer_id", review.customerid);
					command.Parameters.AddWithValue("@product_id", review.productid);
					command.Parameters.AddWithValue("@rating", review.rating);

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
			return true;
		}
	}
}