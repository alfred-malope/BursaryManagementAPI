using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class UniversityRequest
    {
        /*
        University.[Name] AS University, 
        Provinces.ProvinceName AS Province, 
        UniversityFundRequest.Amount,
        [dbo].[Status].[Type] AS [Status],
        UniversityFundRequest.DateCreated,
        UniversityFundRequest.Comment
         */

        public string University { get; set; }
        public string Province { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string Comment { get; set; }
        


        public UniversityRequest(string university, string province, decimal amount, string status, DateTime dateCreated, string comment)
        {
            University = university;
            Province = province;
            Amount = amount;
            Status = status;
            DateCreated = dateCreated;
            Comment = comment;
           
        }

    }
}
