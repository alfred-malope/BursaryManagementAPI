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
                string query = "EXEC [dbo].[GetStudentFundRequests]";
                using (SqlCommand command = new SqlCommand(query, _connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StudentFundRequest request = new StudentFundRequest
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("ID")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            UniversityName = reader.GetString(reader.GetOrdinal("UniversityName")),
                            IDNumber = reader.GetString(reader.GetOrdinal("IDNumber")),
                            BirthDate = reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                            Age = reader.GetByte(reader.GetOrdinal("Age")),
                            GenderName = reader.GetString(reader.GetOrdinal("GenderName")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                            RaceName = reader.GetString(reader.GetOrdinal("RaceName")),
                            Grade = reader.GetByte(reader.GetOrdinal("Grade")),
                            Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                            RequestCreatedDate = reader.GetDateTime(reader.GetOrdinal("RequestCreatedDate")),
                            FundRequestStatus = reader.GetString(reader.GetOrdinal("FundRequestStatus")),
                            DocumentStatus = reader.GetString(reader.GetOrdinal("DocumentStatus")),
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

        public void Create(CreateStudentFundRequestForNewStudent newRequest)
        {
            try
            {
                _connection.Open();
                string query = "EXEC [dbo].[CreateStudentFundRequestForNewStudent] @IDNumber,@FirstName,@LastName,@Email,@PhoneNumber,@GenderName,@RaceName,@UniversityID,@BirthDate,@Grade,@Amount";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@IDNumber", newRequest.IDNumber);
                    command.Parameters.AddWithValue("@FirstName", newRequest.FirstName);
                    command.Parameters.AddWithValue("@LastName", newRequest.LastName);
                    command.Parameters.AddWithValue("@Email", newRequest.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", newRequest.PhoneNumber);
                    command.Parameters.AddWithValue("@GenderName", newRequest.GenderName);
                    command.Parameters.AddWithValue("@RaceName", newRequest.RaceName);
                    command.Parameters.AddWithValue("@UniversityID", newRequest.UniversityID);
                    command.Parameters.AddWithValue("@BirthDate", newRequest.BirthDate);
                    command.Parameters.AddWithValue("@Grade", newRequest.Grade);
                    command.Parameters.AddWithValue("@Amount", newRequest.Amount);

                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                _connection.Close();
            }
        }

        //public void UpdateRequest(int id, StudentFundRequest updatedRequest)
        //{
        //    try
        //    {
        //        _connection.Open();
        //        string query = "UPDATE StudentFundRequest SET StudentID = @StudentID, UniversityID = @UniversityID, Grade = @Grade, Amount = @Amount, ApplicationStatus = @ApplicationStatus, Comment = @Comment WHERE ApplicationID = @ApplicationID";
        //        using (SqlCommand command = new SqlCommand(query, _connection))
        //        {
        //            command.Parameters.AddWithValue("@StudentID", updatedRequest.StudentID);
        //            command.Parameters.AddWithValue("@Grade", updatedRequest.Grade);
        //            command.Parameters.AddWithValue("@Amount", updatedRequest.Amount);
        //            command.Parameters.AddWithValue("@ApplicationStatus", updatedRequest.StatusID);
        //            command.Parameters.AddWithValue("@Comment", updatedRequest.Comment);
        //            command.Parameters.AddWithValue("@ApplicationID", id);

        //            int rowsAffected = command.ExecuteNonQuery();
        //            if (rowsAffected == 0)
        //            {
        //                throw new KeyNotFoundException("Student fund request not found!");
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        _connection.Close();
        //    }
        //}

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