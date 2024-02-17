namespace BusinessLogic.Models
{
    public class StudentFundRequest
    {
        public int ApplicationID { get; set; }
        public int StudentID { get; set; }
        //public byte Grade { get; set; }

        public decimal Amount { get; set; }
        public int ApplicationStatus { get; set; }
        public string? Comment { get; set; }



        // Optional: Additional properties or methods can be added as needed
    }

}
