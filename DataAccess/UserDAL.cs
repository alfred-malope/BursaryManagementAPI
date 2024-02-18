using DataAccess.Entity;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace DataAccess
{
    public class UserDAL(SqlConnection connection)
    {
        SqlConnection _connection = connection;

        public int InsertUserAndGetPrimaryKey(User user)
        {
            _connection.Open();
            string query = "INSERT INTO [dbo].[User] (FirstName, LastName, ContactID) VALUES (@FirstName, @LastName, @ContactID); SELECT SCOPE_IDENTITY()";

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@ContactID", user.ContactID);


                    // Execute the INSERT statement and retrieve ID
                    int primaryKey = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"User ID: '{primaryKey}'");
                    return primaryKey;
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Unable to insert to User Table. Details: '{e.Message}'");
                    throw e;
                }
                finally { _connection.Close(); }


            }
        }
    }
}
