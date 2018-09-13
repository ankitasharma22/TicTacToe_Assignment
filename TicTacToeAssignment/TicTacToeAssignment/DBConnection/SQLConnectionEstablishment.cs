using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToeAssignment.DBConnection
{
    /// <summary>
    /// Class task: establishes connection with the database!
    /// </summary>
    public class SQLConnectionEstablishment
    {
        public SqlConnection SQLConnection()
        {
            SqlConnection connection = new SqlConnection();
            try
            {
                // Build connection string
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "TAVDESK145";   
                builder.UserID = "sa";            
                builder.Password = "test123!@#";      
                builder.InitialCatalog = "TicTacToe";

                // Connect to SQL 
                connection = new SqlConnection(builder.ConnectionString);
                connection.Open(); 
                return connection;
            }
            catch (SqlException e)
            {  
            }
             
            Console.ReadKey(false);
            return connection;
        }
    }
}
