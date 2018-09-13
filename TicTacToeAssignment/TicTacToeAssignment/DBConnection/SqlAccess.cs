using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToeAssignment.DBConnection
{
    public class SqlAccess
    {
        public static void InsertIntoUserTable(string FName, string LName, string UserName)
        {
            SQLConnectionEstablishment connectionString = new SQLConnectionEstablishment();
            SqlConnection connection = connectionString.SQLConnection();

            bool userAlreadyExists = CheckIfUserAlreadyExists(UserName);

            Random random = new Random();
            int TokenID = random.Next(0,1000);

            if (userAlreadyExists)
                throw new Exception("UserName already exists! Try with new UserName!!!");
             
            string query = "Insert into  UserDetails  (FName, LName, UserName, TokenID) values ('" + FName + "', '" + LName + "', '" + UserName + "', '" + TokenID + "')";
            SqlCommand myCommand = new SqlCommand(query, connection);
            myCommand.ExecuteNonQuery();
        }

        public static bool AuthenticateUser(int tokenId)
        {
            SQLConnectionEstablishment connectionString = new SQLConnectionEstablishment();
            SqlConnection connection = connectionString.SQLConnection();

            string query = "SELECT * FROM UserDetails where TokenID = "+tokenId;
            SqlCommand myCommand = new SqlCommand(query, connection);
            SqlDataReader reader = myCommand.ExecuteReader();

            if (reader.HasRows)
                return true;
            else
                return false;
        }

        public static bool CheckIfUserAlreadyExists(string userId)
        {
            SQLConnectionEstablishment connectionString = new SQLConnectionEstablishment();
            SqlConnection connection = connectionString.SQLConnection();

            string query = "SELECT * FROM UserDetails where UserName = '"+userId+"'";
            SqlCommand myCommand = new SqlCommand(query, connection);
            SqlDataReader reader = myCommand.ExecuteReader();

            if (reader.HasRows)
                return true;
            else
                return false;
        }

        public static void LogEntryForRequest(Log log)
        {
            SQLConnectionEstablishment connectionString = new SQLConnectionEstablishment();
            SqlConnection connection = connectionString.SQLConnection(); 
            string query = "Insert into  LogDetails  (Request, Response, Exception, Comments) values ('" + log.Request + "', '" + log.Response + "', '" + log.Exception + "', '" + log.Comment + "')";
            SqlCommand myCommand = new SqlCommand(query, connection);
            myCommand.ExecuteNonQuery();
        }
    }
}
