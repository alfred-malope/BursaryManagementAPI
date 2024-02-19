using DataAccess.Entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
  public class AdminDAL
    {
      private readonly SqlConnection _connection;

      public AdminDAL(SqlConnection connection)
        {
          _connection = connection;
      }


    

        public IEnumerable<UniversityRequest>? GetAllUniversityFundRequests()
        {
        try
          {
            _connection.Open();
            List<UniversityRequest> requests = new List<UniversityRequest>();
            string query = "SELECT * FROM [dbo].[vw_UniversityRequests]";
            using (SqlCommand command = new SqlCommand(query, _connection))
            using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UniversityRequest universityRequest = new UniversityRequest(
                                        reader.GetString(0),
                                        reader.GetString(1),
                                        reader.GetDecimal(2),
                                        reader.GetString(3),
                                        reader.GetDateTime(4),
                                        reader.GetString(5)                                                                                                                                   );
                        requests.Add(universityRequest);
                    }
                }
            return requests;
          }
        catch (Exception ex)
            {
                throw new Exception($"Error getting university requests connection problems{ex.Message}/n {ex.StackTrace}");
            }
        finally
            {
                _connection.Close();
            }
          }



        public UniversityRequest? UpdateUniversityFundRequest(int requestID, int statusID)
        {
            try
            {
                _connection.Open();
                UniversityRequest? request = null;
                string query = "EXEC [dbo].[usp_UpdateUniversityFundRequest] @RequestID, @StatusID";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@RequestID", requestID);
                    command.Parameters.AddWithValue("@StatusID", statusID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            request = new UniversityRequest(
                                                           reader.GetString(0),
                                                           reader.GetString(1),
                                                           reader.GetDecimal(2),
                                                           reader.GetString(3),
                                                           reader.GetDateTime(4),
                                                           reader.GetString(5)
                                                           );
                        }
                    }
                }
                return request;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating university fund request connection problems");
            }
            finally
            {
                _connection.Close();
            }
        }

     

        public UniversityRequest? NewUniversityFundRequest(int universityID, decimal amount, string comment)
        {
            try
            {
                _connection.Open();
                UniversityRequest? request = null;
                string query = "EXEC [dbo].[usp_NewUniversityFundRequest] @UniversityID, @Amount, @Comment";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@UniversityID", universityID);
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.Parameters.AddWithValue("@Comment", comment);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            request = new UniversityRequest(
                                        reader.GetString(0),
                                        reader.GetString(1),
                                        reader.GetDecimal(2),
                                        reader.GetString(3),
                                        reader.GetDateTime(4),
                                        reader.GetString(5)
                                        );
                        }
                    }
                }
                return request;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating new university fund request connection problems");
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
