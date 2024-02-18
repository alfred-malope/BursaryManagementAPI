
using System.ComponentModel.DataAnnotations;


namespace BursaryManagementAPI.Models.DataModels
{
    public class User 
    {

        [Required]
        [MaxLength(120)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(120)]
        public string LastName { get; set; }

        // Foreign Key to ContactDetails table
        public int ContactID { get; set; }

        
    }
}
