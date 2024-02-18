﻿using BursaryManagementAPI.Models.DataModels;
using Microsoft.Data.SqlClient;
using System.Data;



namespace BursaryManagementAPI
{
    public class DBManager
    {


        // constructor
        private readonly SqlConnection _connection;

        public DBManager(SqlConnection connection)
        {
            _connection =connection;
        }


        public void OpenConnection() => _connection.Open();

        public void CloseConnection() => _connection.Close();

        public void ExecuteNonQuery(string query)
        {
            SqlCommand command = new SqlCommand(query, _connection);
            command.ExecuteNonQuery();
        }

        public SqlDataReader ExecuteReader(string query)
        {
            SqlCommand command = new SqlCommand(query, _connection);
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }

        public int InsertUserAndGetPrimaryKey(User user)
        {
            using (SqlCommand command = new SqlCommand { Connection = _connection })
            {
                command.CommandText = "INSERT INTO User (FirstName, LastName, ContactID) VALUES (@FirstName, @LastName, @ContactID)";
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@ContactID", user.ContactID);


                // Execute the INSERT statement
                command.ExecuteNonQuery();
                // Retrieve the last inserted ID using SCOPE_IDENTITY()
                command.CommandText = "SELECT SCOPE_IDENTITY()";
                int primaryKey = Convert.ToInt32(command.ExecuteScalar());
                Console.WriteLine($"User ID: '{primaryKey}'");
                return primaryKey;

            }
        }
        public SqlConnection GetSqlConnection()
        {
            return _connection;
        }
        public int InsertContactsAndGetPrimaryKey(ContactDetails contactDetails)
        {
            using (SqlCommand command = new SqlCommand { Connection = _connection})
            {
                command.CommandText = "INSERT INTO ContactDetails (Email, PhoneNumber) VALUES (@Email, @PhoneNumber)";
                command.Parameters.AddWithValue("@Email", contactDetails.Email);
                command.Parameters.AddWithValue("@PhoneNumber", contactDetails.PhoneNumber);

                // Execute the INSERT statement
                command.ExecuteNonQuery();
                // Retrieve the last inserted ID using SCOPE_IDENTITY()
                command.CommandText = "SELECT SCOPE_IDENTITY()";
                int primaryKey = Convert.ToInt32(command.ExecuteScalar());
                Console.WriteLine($"Contact ID: '{primaryKey}'");

                return primaryKey;

            }
        }
        // list of all the provinces
        public List<Province> GetProvinces()
        {
            List<Province> provinces = new List<Province>();
            string query = "SELECT * FROM Province";
            SqlDataReader reader = ExecuteReader(query);

            // IF there are rows in the table add them to the list
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Province province = new Province(
                                       _id: int.Parse(reader.GetString(0)),
                                       _provinceName: reader.GetString(1)
                                       );
                    provinces.Add(province);
                }
            }
            reader.Close();
            return provinces;
        }


        // list of all the universities



        public List<BBDAllocation> BBDAllocations()
        {


            string query = "SELECT * FROM BBDAllocation";
            SqlDataReader reader = ExecuteReader(query);
            List<BBDAllocation> allocations = new List<BBDAllocation>();

            while (reader.Read())
            {

                BBDAllocation allocation = new(
                            _id: reader.GetInt32(0),
                            _budget: (decimal)reader.GetSqlMoney(1),
                            _dateCreated: reader.GetDateTime(2)
                            );
                Console.WriteLine(allocation.getBudget());
                allocations.Add(allocation);
            }

            return allocations;
        }

        // GET BBD ALLOCATION BY YEAR
        public BBDAllocation? GetBBDAllocationByYear(int Year) => BBDAllocations()[0];


        public void SaveUniversityFundAllocation(UniversityFundAllocation allocation)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = _connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO UniversityFundAllocation (Budget, DateAllocated, UniversityID, BBDAllocationID) VALUES (@Budget, @DateAllocated, @UniversityID, @BBDAllocationID)";
                Console.WriteLine(allocation.getBudget());
                command.Parameters.AddWithValue("@Budget", allocation.getBudget());
                command.Parameters.AddWithValue("@DateAllocated", allocation.getDateAllocated());
                command.Parameters.AddWithValue("@UniversityID", allocation.getUniversityID());
                command.Parameters.AddWithValue("@BBDAllocationID", allocation.getBBDAllocationID());
                command.ExecuteNonQuery();
            }
        }

    }
}

   


