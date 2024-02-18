using BursaryManagementAPI.Models.DataModels;
using Microsoft.Data.SqlClient;

namespace BursaryManagementAPI.DataAccess.DAO
{
    public class ContactsDAO(DBManager dbManager)
    {
        DBManager _dbManager = dbManager;

        public int InsertContactsAndGetPrimaryKey(ContactDetails contactDetails)
        {   _dbManager.OpenConnection();
            string query = "INSERT INTO ContactDetails (Email, PhoneNumber) VALUES (@Email, @PhoneNumber); SELECT SCOPE_IDENTITY()";
            using (SqlCommand command = new SqlCommand(query,_dbManager.GetSqlConnection()))
            {
                try
                {
                    Console.WriteLine($"Contacts: {contactDetails.Email} {contactDetails.PhoneNumber}");
                    command.Parameters.AddWithValue("@Email", contactDetails.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", contactDetails.PhoneNumber);
                    Console.WriteLine(command.CommandText);
                    // Execute the INSERT statement and retrieve ID
                    int primaryKey = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"Contact ID: '{primaryKey}'");
                    return primaryKey;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to insert to ContactDetails Table. Details: '{ex.StackTrace}'");
                    throw ex;
                }
                finally { _dbManager.CloseConnection(); }

            }
        }
    }
}

