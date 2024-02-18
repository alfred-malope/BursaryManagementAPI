using System.ComponentModel.DataAnnotations;

namespace BursaryManagementAPI.Models.DTO
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email {  get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
