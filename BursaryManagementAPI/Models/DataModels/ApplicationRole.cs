using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BursaryManagementAPI.Models.DataModels
{
    [Table("Role")]
    public class ApplicationRole : IdentityRole<int>
    {
        public string RoleType { get; set; }
    }
}
