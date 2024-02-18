﻿using DataAccess.Entity;
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

        public void UpdateRequest(int id, UpdateStudentFundRequest updatedRequest)
        {
            try
            {
                _connection.Open();
                string query = "UPDATE StudentFundRequest SET Grade = @Grade, Amount = @Amount WHERE ID = @ID AND StatusID = 3";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Grade", updatedRequest.Grade);
                    command.Parameters.AddWithValue("@Amount", updatedRequest.Amount);
                    command.Parameters.AddWithValue("@ID", id);

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

        public void UpdateApplicationStatus(int applicationId, int status, string comment)
        {
            try
            {
                _connection.Open();

                if (status == 2 && string.IsNullOrWhiteSpace(comment))
                {
                    throw new ArgumentException("A comment is required when changing the status to 2.");
                }

                string query;

                if (!string.IsNullOrWhiteSpace(comment))
                {
                    query = "UPDATE StudentFundRequest SET StatusID = @Status, Comment = @Comment WHERE ID = @ID";
                }
                else
                {
                    query = "UPDATE StudentFundRequest SET StatusID = @Status WHERE ID = @ID";
                }

                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@ID", applicationId);

                    if (!string.IsNullOrWhiteSpace(comment))
                    {
                        command.Parameters.AddWithValue("@Comment", comment);
                    }

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