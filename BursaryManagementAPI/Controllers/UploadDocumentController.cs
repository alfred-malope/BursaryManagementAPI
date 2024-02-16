using BursaryManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace BursaryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadDocumentController : ControllerBase
    {
        private readonly SqlConnection _connection;

        public UploadDocumentController(SqlConnection connection)
        {
            _connection = connection;
        }

        [HttpPost("{applicationId}/upload")]
        public ActionResult UploadDocument(int applicationId, [FromBody] Document document)
        {
            try
            {
                _connection.Open();

                string query = "INSERT INTO Document (ApplicationID, DocumentType, DocumentPath) VALUES (@ApplicationID, @DocumentType, @DocumentPath)";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);
                    command.Parameters.AddWithValue("@DocumentType", document.DocumentType);
                    command.Parameters.AddWithValue("@DocumentPath", document.DocumentPath);

                    command.ExecuteNonQuery();
                }

                return Ok("Document uploaded successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading document: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
