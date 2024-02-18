using BursaryManagementAPI.Models.DataModels;
using Microsoft.Data.SqlClient;

namespace BursaryManagementAPI.DataAccess.DAO
{
    public class UserDAO(DBManager dbManager)
    {
        DBManager _dbManager = dbManager;

        public int InsertUserAndGetPrimaryKey(User user)
        {
            _dbManager.OpenConnection();                    
            string query = "INSERT INTO [dbo].[User] (FirstName, LastName, ContactID) VALUES (@FirstName, @LastName, @ContactID); SELECT SCOPE_IDENTITY()";

            using (SqlCommand command = new SqlCommand(query,_dbManager.GetSqlConnection()))
            {
                try
                {
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@ContactID", user.ContactID);


                    // Execute the INSERT statement and retrieve ID
                    int primaryKey = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"User ID: '{primaryKey}'");
                    _dbManager.CloseConnection();
                    return primaryKey;
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Unable to insert to User Table. Details: '{e.Message}'");
                    throw e;
                }
                finally { _dbManager.CloseConnection(); }


            }
        }
    }
}
