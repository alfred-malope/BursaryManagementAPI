


using DataAccess.Entity;
using Microsoft.Data.SqlClient;
using System.Data;


public class UniversityDAL(SqlConnection connection)
{
    private List<University> universities;
    SqlConnection _connection = connection;

    public List<University> GetUniversities()
    {
        _connection.Open();
        List<University> universities = new List<University>();
        string query = "SELECT * FROM University";
        SqlDataReader reader = new SqlCommand(query,_connection).ExecuteReader();


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
        _connection.Close();

        return universities;
    }

    public void allocate()
    {
        _connection.Open();
        // get all the universities ids only stored in a list using linq
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
            new UniversityFundAllocation(budget, DateTime.Now, id, allocation.getID()).save();
        });
        _connection.Close();

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
}

