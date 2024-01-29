using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data
{
    public class Data
    {
        //Adjust this accordingly to connect the database on your system
        //protected static readonly string connectionString = @"Server=localhost;Database=ShoppingCartDB;User Id=SA;Password=MyPass@word;Trust Server Certificate=True";
        protected static readonly string connectionString = @"Server=(local);Database=ShoppingCartDB;Integrated Security=true;encrypt=false";
    }
}
