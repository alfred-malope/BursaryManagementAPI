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

        public User getUserByEmail(string email)
        {
            _connection.Open();
            User user = new User();

            string query = "SELECT usr.ID, FirstName, LastName,ContactDetails.ID, PhoneNumber, Email FROM [dbo].[User] as usr INNER JOIN ContactDetails ON ContactDetails.Email=@Email AND ContactDetails.ID = usr.ContactID";
            using(SqlCommand command = new SqlCommand(query, _connection))
            {
                try
                {  command.Parameters.AddWithValue("@Email", email);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        user = new User
                        {
                            ID = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            ContactID = reader.GetInt32(3),
                        };

                    }
                }
                catch (Exception e)
                {
                    _connection.Close();
                    throw e;
                }
            }
            _connection.Close();
            return user;
        }
    }
}
