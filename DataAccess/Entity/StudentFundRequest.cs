namespace DataAccess.Entity
{
    public class StudentFundRequest
    {
        public int ApplicationID { get; set; }
        public int StudentID { get; set; }
        public int UniversityID { get; set; }
        public int Grade { get; set; }
        public decimal Amount { get; set; }
        public int ApplicationStatus { get; set; }
        public string Comment { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UniversityName { get; set; }
        public string StatusName { get; set; }
        public string StatusDescription { get; set; }
        public string StatusColor { get; set; }
        public string StatusIcon { get; set; }
        public string StatusTextColor { get; set; }


        // Optional: Additional properties or methods can be added as needed
    }

}
