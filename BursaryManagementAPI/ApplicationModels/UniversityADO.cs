using example.DataAccess;
using example.DataAccess.models;

namespace example.ApplicationModels
{
    public static class UniversityADO
    {
        private static List<University> universities;
        public static List<University> getUniversities() => new DBManager().GetUniversities();
    }
}
