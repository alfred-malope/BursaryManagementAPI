using DataAccess;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;


namespace DataAccessLayerTests
{
    public class UnitTest1
    { string connectionString = @"Data Source=.;Integrated Security=True";
        [Fact]
        public void TextConnection()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                new SqlCommand("Create DATABASE #UkukhulaDBTest;", connection).ExecuteNonQuery(); 
                // Create a temporary table
                TestsData testsData = new TestsData();
                using (var command = new SqlCommand(testsData.createTables, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Insert data into the temporary table
                using (var command = new SqlCommand(testsData.seedData, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
                var universities = new UniversityDAL(connection).GetUniversities();
                Assert.Equal(0, universities.Count);

                using (var command = new SqlCommand(testsData.dropdatabe, connection))
                {
                    command.ExecuteNonQuery();
                }

            }
        }
    }
}