

using BursaryManagementAPI;
using BursaryManagementAPI.Models.DataModels;
using Microsoft.Data.SqlClient;


public class UniversityDAO(DBManager dbManager)
{
    private List<University> universities;
    DBManager _dbManager = dbManager;

    public List<University> GetUniversities()
    {
        _dbManager.OpenConnection();
        List<University> universities = new List<University>();
        string query = "SELECT * FROM University";
        SqlDataReader reader = _dbManager.ExecuteReader(query);


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
        _dbManager.CloseConnection();

        return universities;
    }

    public void allocate()
    {
        _dbManager.OpenConnection();
        // get all the universities ids only stored in a list using linq
        List<int> universityIDs = GetUniversities().Select(u => u.GetID()).ToList();

        int numberOfInstitutions = universityIDs.Count();
        BBDAllocation? allocation = _dbManager.GetBBDAllocationByYear(DateTime.Now.Year);
        if (allocation == null)
        {
            throw new Exception("No allocation for the year");
        }
        decimal budget = allocation.getBudget() / numberOfInstitutions;
        universityIDs.ForEach(id =>
        {
            new UniversityFundAllocation(budget, DateTime.Now, id, allocation.getID()).save();
        });
        _dbManager.CloseConnection();

    }
}

