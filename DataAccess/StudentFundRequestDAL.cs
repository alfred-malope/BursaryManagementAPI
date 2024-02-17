using DataAccess.Entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace DataAccess
{
    public class StudentFundRequestDAL
    {
        private readonly SqlConnection _connection;

        public StudentFundRequestDAL(SqlConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<StudentFundRequest> GetAllRequests()
        {
            try
            {
                _connection.Open();
                List<StudentFundRequest> requests = new List<StudentFundRequest>();
                string query = "SELECT * FROM StudentFundRequest";
                using (SqlCommand command = new SqlCommand(query, _connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StudentFundRequest request = new StudentFundRequest
                        {
                            ApplicationID = reader.GetInt32(reader.GetOrdinal("ID")),
                            Grade = reader.GetByte(reader.GetOrdinal("Grade")),
                            Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                            StatusID = reader.GetInt32(reader.GetOrdinal("StatusID")),
                            Comment = reader.IsDBNull(reader.GetOrdinal("Comment")) ? "" : reader.GetString(reader.GetOrdinal("Comment"))
                        };
                        requests.Add(request);
                    }
                }
                return requests;
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Create(StudentFundRequest newRequest)
        {
            try
            {
                _connection.Open();
                string query = "INSERT INTO StudentFundRequest (StudentID, UniversityID, Grade, Amount) VALUES (@StudentID, @UniversityID, @Grade, @Amount)";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@StudentID", newRequest.StudentID);
                    //command.Parameters.AddWithValue("@Grade", newRequest.Grade);
                    command.Parameters.AddWithValue("@Amount", newRequest.Amount);

                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                _connection.Close();
            }
        }

        public void UpdateRequest(int id, StudentFundRequest updatedRequest)
        {
            try
            {
                _connection.Open();
                string query = "UPDATE StudentFundRequest SET StudentID = @StudentID, UniversityID = @UniversityID, Grade = @Grade, Amount = @Amount, ApplicationStatus = @ApplicationStatus, Comment = @Comment WHERE ApplicationID = @ApplicationID";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@StudentID", updatedRequest.StudentID);
                    //command.Parameters.AddWithValue("@Grade", updatedRequest.Grade);
                    command.Parameters.AddWithValue("@Amount", updatedRequest.Amount);
                    //command.Parameters.AddWithValue("@ApplicationStatus", updatedRequest.ApplicationStatus);
                    command.Parameters.AddWithValue("@Comment", updatedRequest.Comment);
                    command.Parameters.AddWithValue("@ApplicationID", id);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new KeyNotFoundException("Student fund request not found!");
                    }
                }
            }
            finally
            {
                _connection.Close();
            }
        }

        public void UpdateApplicationStatus(int applicationId, int status)
        {
            try
            {
                _connection.Open();
                string query = "UPDATE StudentFundRequest SET ApplicationStatus = @Status WHERE ApplicationID = @ApplicationID";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);

                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}