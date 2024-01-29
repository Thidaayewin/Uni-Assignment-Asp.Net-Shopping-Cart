using Microsoft.Data.SqlClient;
using ShoppingCart.Models;
using System.Data;
using Microsoft.AspNetCore.DataProtection;
using Azure.Core;

namespace ShoppingCart.Data
{
    public class AccountData : Data
    {
        private readonly IDataProtector _dataProtectionProvider;

        public AccountData(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider.CreateProtector(purpose: "UserIdProtection"); ;

        }

        public static LoginResult GetCustomerInfo(string username, string encryptPassword, string sessionId)
        {
            LoginResult customer = new LoginResult();
            // Define your stored procedure name
            string storedProcedureName = "select_customer_login_info";

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
                    command.Parameters.AddWithValue("@email", username);
                    command.Parameters.AddWithValue("@password", encryptPassword);
                    command.Parameters.AddWithValue("@session_id", sessionId);

                    // Execute the stored procedure
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        LoginResult cus = new LoginResult()
                        {
                            status_code = (int)reader["status_code"]
                        };
                        customer = cus;
                    }
                    // Close the SqlDataReader object
                    reader.Close();
                }
            }
            return customer;
        }

        public LoginResult CheckCredentail(string username, string password, string sessionId)
        {
            var encrypt = password;
            LoginResult customer = GetCustomerInfo(username, encrypt, sessionId);
            return customer;
        }

        public Customer RetrieveCustomerNameID(string username) //retrieves customer's details in the DB upon successful login
        {
            Customer customer = new Customer();

            // Define your stored procedure name
            string storedProcedureName = "getCustomerNameID";

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
                    command.Parameters.AddWithValue("@email", username);

                    // Execute the stored procedure
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        customer.customerID = (int)reader["customer_id"];
                        customer.firstName = (string)reader["first_name"];
                        customer.lastName = (string)reader["last_name"];

                    }
                    // Close the SqlDataReader object
                    reader.Close();
                }
            }

            return customer;
        }
    }
}
