using BursaryManagementAPI.Models.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BursaryManagementAPI.Models.DataModels
{
    public class Document
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(250)]
        public string DocumentPath {  get; set; }

        [ForeignKey("ID")]
        public DocumentType TypeID { get; set; }


        [ForeignKey("ID")]
        public StudentFundRequest RequestID { get; set; }
    }
}
