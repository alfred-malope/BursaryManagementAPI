

using BursaryManagementAPI;
using BursaryManagementAPI.Models.DataModels;
using Microsoft.Data.SqlClient;


public class UniversityDAO
{
    private List<University> universities;
    DBManager _dbManager;

    public UniversityDAO(DBManager dbManager)
    {
        _dbManager = dbManager;
    }

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

}

