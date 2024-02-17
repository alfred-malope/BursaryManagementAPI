using Microsoft.AspNetCore.Http;

namespace BursaryManagementAPI.Models.DTO
{
    public class UploadDocument
    {

        public required IFormFile File { get; set; }
        public int DocumentType { get; set; }

    }
}
