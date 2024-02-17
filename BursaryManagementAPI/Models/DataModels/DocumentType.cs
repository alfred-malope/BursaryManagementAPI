using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace BursaryManagementAPI.Models.DataModels
{
    public class DocumentType
    {
        [Key]
        public int ID { get; set; }

        
        [StringLength(20)]
        public string Type { get; set; }

    }
}
