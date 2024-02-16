using BursaryManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace BursaryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentFundRequestController : ControllerBase
    {
        private readonly SqlConnection _connection;

        public StudentFundRequestController(SqlConnection connection)
        {
            _connection = connection;
        }

        [HttpGet]
        public ActionResult<IEnumerable<StudentFundRequest>> GetAllRequests()
        {
            try
            {
                _connection.Open();

                List<StudentFundRequest> requests = new List<StudentFundRequest>();
                string query = "SELECT * FROM StudentFundRequest";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StudentFundRequest request = new StudentFundRequest
                            {
                                ApplicationID = reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                                UniversityID = reader.GetInt32(reader.GetOrdinal("UniversityID")),
                                Grade = reader.GetInt32(reader.GetOrdinal("Grade")),
                                Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                                ApplicationStatus = reader.GetInt32(reader.GetOrdinal("ApplicationStatus")),
                                Comment = reader.IsDBNull(reader.GetOrdinal("Comment")) ? "" : reader.GetString(reader.GetOrdinal("Comment"))
                            };
                            requests.Add(request);
                        }
                    }
                }

                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving student fund requests: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        [HttpPost]
        public ActionResult Create([FromBody] StudentFundRequest newRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _connection.Open();

                string query = "INSERT INTO StudentFundRequest (StudentID, UniversityID, Grade, Amount) VALUES (@StudentID, @UniversityID, @Grade, @Amount)";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    // Add parameters to the command
                    command.Parameters.AddWithValue("@StudentID", newRequest.StudentID);
                    command.Parameters.AddWithValue("@UniversityID", newRequest.UniversityID);
                    command.Parameters.AddWithValue("@Grade", newRequest.Grade);
                    command.Parameters.AddWithValue("@Amount", newRequest.Amount);

                    command.ExecuteNonQuery();
                }

                return Ok("Student fund request created successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating student fund request: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        [HttpPut("{id}")]
        public ActionResult UpdateRequest(int id, [FromBody] StudentFundRequest updatedRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _connection.Open();

                string query = "UPDATE StudentFundRequest SET StudentID = @StudentID, UniversityID = @UniversityID, Grade = @Grade, Amount = @Amount, ApplicationStatus = @ApplicationStatus, Comment = @Comment WHERE ApplicationID = @ApplicationID";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@StudentID", updatedRequest.StudentID);
                    command.Parameters.AddWithValue("@UniversityID", updatedRequest.UniversityID);
                    command.Parameters.AddWithValue("@Grade", updatedRequest.Grade);
                    command.Parameters.AddWithValue("@Amount", updatedRequest.Amount);
                    command.Parameters.AddWithValue("@ApplicationStatus", updatedRequest.ApplicationStatus);
                    command.Parameters.AddWithValue("@Comment", updatedRequest.Comment);
                    command.Parameters.AddWithValue("@ApplicationID", id);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        return NotFound("Student fund request not found!");
                    }
                }

                return Ok("Student fund request updated successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating student fund request: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }
        [HttpPost("{applicationId}/approve")]
        public ActionResult ApproveApplication(int applicationId)
        {
            try
            {
                _connection.Open();

                string query = "UPDATE StudentFundRequest SET ApplicationStatus = @Status WHERE ApplicationID = @ApplicationID";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Status", 1); // 1 indicates "Approved"
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);

                    command.ExecuteNonQuery();
                }

                return Ok("Application approved successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error approving application: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        [HttpPost("{applicationId}/reject")]
        public ActionResult RejectApplication(int applicationId)
        {
            try
            {
                _connection.Open();

                string query = "UPDATE StudentFundRequest SET ApplicationStatus = @Status WHERE ApplicationID = @ApplicationID";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Status", 2); // 2 indicates "Rejected"
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);

                    command.ExecuteNonQuery();
                }

                return Ok("Application rejected successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error rejecting application: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }


    }
}

