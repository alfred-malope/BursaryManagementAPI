using BursaryManagementAPI.Models.DataModels;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;



namespace BursaryManagementAPI
{
    public class DBManager
    {

        public SqlConnection _connection =
            new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BURSARYDB;Integrated Security=True;Connect Timeout=30");

        // constructor
        private readonly SqlConnection _connection;

        public DBManager(SqlConnection connection)
        {
            _connection =connection;
        }


        public void OpenConnection() => _connection.Open();

        public void CloseConnection() => _connection.Close();

        public void ExecuteNonQuery(string query)
        {
            SqlCommand command = new SqlCommand(query, _connection);
            command.ExecuteNonQuery();
        }

        public SqlDataReader ExecuteReader(string query)
        {
            SqlCommand command = new SqlCommand(query, _connection);
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }


        // list of all the provinces
        public List<Province> GetProvinces()
        {
            List<Province> provinces = new List<Province>();
            string query = "SELECT * FROM Province";
            SqlDataReader reader = ExecuteReader(query);

            // IF there are rows in the table add them to the list
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Province province = new Province(
                                       _id: int.Parse(reader.GetString(0)),
                                       _provinceName: reader.GetString(1)
                                       );
                    provinces.Add(province);
                }
            }
            reader.Close();
            CloseConnection();
            return provinces;
        }


        // list of all the universities
        public List<University> GetUniversities()
        {
            List<University> universities = new List<University>();
            string query = "SELECT * FROM University";
            SqlDataReader reader = ExecuteReader(query);


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


        public List<BBDAllocation> BBDAllocations()
        {


            string query = "SELECT * FROM BBDAllocation";
            SqlDataReader reader = ExecuteReader(query);
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
    }
}

   


