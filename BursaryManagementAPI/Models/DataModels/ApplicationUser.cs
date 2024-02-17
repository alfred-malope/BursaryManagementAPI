using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BursaryManagementAPI.Models.DataModels
{
    [Table("User")]
    public class ApplicationUser : IdentityUser<int>
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(120)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(120)]
        public string LastName { get; set; }

        // Foreign Key to ContactDetails table
        public int ContactID { get; set; }

        
        [ForeignKey("ContactID")]
        public ContactDetails ContactDetails { get; set; }


        // Navigation property for the ContactDetails relationship
        public ICollection<UserRole> UserRole { get; set; }
    }
}
