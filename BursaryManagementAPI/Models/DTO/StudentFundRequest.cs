using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BursaryManagementAPI.Models.DTO
{
    public class StudentFundRequest
    {
        
        public int ID { get; set; }

        
        public int StudentID { get; set; }
        public int CreatedDate { get; set; }
        public int ModifiedDate { get; set; }
        public int Grade { get; set; }
        public decimal Amount { get; set; }
        public int ApplicationStatus { get; set; }
        public string? Comment { get; set; }

        // Optional: Additional properties or methods can be added as needed
    }

}
