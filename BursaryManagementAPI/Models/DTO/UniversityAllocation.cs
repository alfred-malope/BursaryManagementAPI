

namespace BursaryManagementAPI.Models.DTO
{
    public class UniversityAllocation
    {
        string UniversityName { get; set; }
        double AllocatedBudget { get; set; }
        int year { get; set; }

        public UniversityAllocation(string _universityName, double _allocatedBudget, int _year)
        {
            UniversityName = _universityName;
            AllocatedBudget = _allocatedBudget;
            year = _year;
        }

        
    }
}
