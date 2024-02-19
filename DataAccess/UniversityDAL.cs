
using DataAccess.Entity;
using Microsoft.Data.SqlClient;
using System.Data;



public class UniversityDAL(SqlConnection connection)
{
    private List<University> universities;
    SqlConnection _connection = connection;

    public List<University> GetUniversities()
    {
        
        List<University> universities = new List<University>();
        string query = "SELECT * FROM University";
        SqlDataReader reader = new SqlCommand(query, _connection).ExecuteReader();


        while (reader.Read())
        {
            University university = new(
                        _id: reader.GetInt32(0),
                       _name: reader.GetString(1),
                       _provinceID: reader.GetInt32(2)
                       );

            universities.Add(university);
        }


        reader.Close();
        

        return universities;
    }

    public void allocate()
    {

        List<int> universityIDs = GetUniversities().Select(u => u.GetID()).ToList();
        int numberOfInstitutions = universityIDs.Count();
        BBDAllocation? allocation = GetBBDAllocationByYear(DateTime.Now.Year);
        if (allocation == null)
        {
            throw new Exception("No allocation for the year");
        }
        decimal budget = allocation.getBudget() / numberOfInstitutions;
        universityIDs.ForEach(id =>
        {
            var universityFundAllocation = new UniversityFundAllocation(budget, DateTime.Now, id, allocation.getID());
            SaveUniversityFundAllocation(universityFundAllocation);
        });
        connection.Close();

    }


    public List<BBDAllocation> BBDAllocations()
    {


        string query = "SELECT * FROM BBDAllocation";
        SqlDataReader reader = new SqlCommand(query, _connection).ExecuteReader();
        List<BBDAllocation> allocations = new List<BBDAllocation>();

        while (reader.Read())
        {

            BBDAllocation allocation = new(
                        _id: reader.GetInt32(0),
                        _budget: (decimal)reader.GetSqlMoney(1),
                        _dateCreated: reader.GetDateTime(2)
                        );
            Console.WriteLine(allocation.getBudget());
            allocations.Add(allocation);
        }
        reader.Close();
        return allocations;
    }

    // GET BBD ALLOCATION BY YEAR
    public BBDAllocation? GetBBDAllocationByYear(int Year) => BBDAllocations()[0];


    public void SaveUniversityFundAllocation(UniversityFundAllocation allocation)
    {
        using (SqlCommand command = new SqlCommand())
        {
            command.Connection = _connection;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO UniversityFundAllocation (Budget, DateAllocated, UniversityID, BBDAllocationID) VALUES (@Budget, @DateAllocated, @UniversityID, @BBDAllocationID)";
            Console.WriteLine(allocation.getBudget());
            command.Parameters.AddWithValue("@Budget", allocation.getBudget());
            command.Parameters.AddWithValue("@DateAllocated", allocation.getDateAllocated());
            command.Parameters.AddWithValue("@UniversityID", allocation.getUniversityID());
            command.Parameters.AddWithValue("@BBDAllocationID", allocation.getBBDAllocationID());
            command.ExecuteNonQuery();
        }
    }

    public void SaveUniversityFundRequest(UniversityFundRequest request)
    {
        using (SqlCommand command = new SqlCommand())
        {
            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO UniversityFundRequest (UniversityID, DateCreated, Amount, StatusID, Comment) VALUES (@UniversityID, @DateCreated, @Amount, @StatusID, @Comment)";
            command.Parameters.AddWithValue("@UniversityID", request.getUniversityID());
            command.Parameters.AddWithValue("@DateCreated", request.getDateCreated());
            command.Parameters.AddWithValue("@Amount", request.getAmount());
            command.Parameters.AddWithValue("@StatusID", request.getStatusID());
            command.Parameters.AddWithValue("@Comment", request.getComment());
            command.ExecuteNonQuery();
        }
    }

    public List<UniversityFundRequest> GetUniversityFundRequests()
    {
        connection.Open();
        List<UniversityFundRequest> requests = new List<UniversityFundRequest>();
        string query = "SELECT * FROM UniversityFundRequest";
        SqlDataReader reader = new SqlCommand(query, connection).ExecuteReader();

        while (reader.Read())
        {
            UniversityFundRequest request = new(
                                      universityID: reader.GetInt32(0),
                                      dateCreated: reader.GetDateTime(2),
                                      amount: (decimal)reader.GetSqlMoney(3),
                                      statusID: reader.GetString(4),
                                      comment: reader.GetString(5)
                                                                                                                                                                                 );
            requests.Add(request);
        }

        reader.Close();
        connection.Close();

        return requests;
    }

    public void UpdateUniversityFundRequestStatusAndComment(int id, string statusID, string comment)
    {
        connection.Open();
        string query = "UPDATE UniversityFundRequest SET StatusID = @StatusID, Comment = @Comment WHERE ID = @ID";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@StatusID", statusID);
        command.Parameters.AddWithValue("@Comment", comment);
        command.Parameters.AddWithValue("@ID", id);
        command.ExecuteNonQuery();
        connection.Close();
    }
}

