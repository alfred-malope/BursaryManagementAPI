namespace example.DataAccess.models
{
    public class UniversityFundRequest
    {
        int ID { get; set; } 
        int UniversityID { get; set; }
        DateTime DateCreated { get; set; }
        Double Amount { get; set; }
        string StatusID { get; set; }
        string Comment { get; set; }


        public UniversityFundRequest(int universityID, DateTime dateCreated, Double amount, string statusID, string comment)
        {
            UniversityID = universityID;
            DateCreated = dateCreated;
            Amount = amount;
            StatusID = statusID;
            Comment = comment;
        }

        public void save()
        {
            //new DBManager().SaveUniversityFundRequest(this);
        }
    }
}
