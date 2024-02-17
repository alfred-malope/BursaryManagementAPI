using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using BursaryManagementAPI.Models.DTO;

namespace BursaryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadDocumentController : ControllerBase
    {

        private readonly SqlConnection _connection;
        private readonly BlobServiceClient _blobServiceClient;

        public UploadDocumentController(SqlConnection connection, BlobServiceClient blobServiceClient)
        {
            _connection = connection;
            _blobServiceClient = blobServiceClient;
        }

        [HttpPost("{applicationId}/upload")]
        public async Task<ActionResult> UploadDocument(int applicationId, [FromForm] UploadDocument uploadDocument)
        {
            try
            {
                var file = uploadDocument.File;
                if (file == null || file.Length == 0)
                    return BadRequest("File is empty");

                await _connection.OpenAsync();

                // Generate unique file name
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                // Upload file to Azure Blob Storage
                var blobClient = _blobServiceClient.GetBlobContainerClient("bursarymanagementcontainer").GetBlobClient(uniqueFileName);
                await blobClient.UploadAsync(file.OpenReadStream());

                // Save file path to the database
                string query = "INSERT INTO Document (RequestID, TypeID, Document) VALUES (@ApplicationID, @DocumentType, @DocumentPath)";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);
                    command.Parameters.AddWithValue("@DocumentType", uploadDocument.DocumentType); 
                    command.Parameters.AddWithValue("@DocumentPath", blobClient.Uri.ToString()); 
                    await command.ExecuteNonQueryAsync();
                }

                return Ok("File uploaded successfully!");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in a more specific way
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading document: {ex.Message}");
            }
            finally
            {
                if (_connection.State != System.Data.ConnectionState.Closed)
                    _connection.Close();
            }
        }
    }
}
