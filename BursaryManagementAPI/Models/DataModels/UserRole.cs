namespace BursaryManagementAPI.Models.DataModels
{
    public class UserRole
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }

        // Navigation properties (optional)
        public ApplicationUser User { get; set; }
        public ApplicationRole Role { get; set; }

    }
}
