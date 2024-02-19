using BusinessLogic.Models.Response;
using DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BursaryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionController : ControllerBase
    {
        private readonly UniversityDAL _universityDAL;
        private readonly SqlConnection _connection; 

        public InstitutionController(UniversityDAL universityDAL, SqlConnection connection)
        {
            _universityDAL = universityDAL;
            _connection = connection;
        }
        [HttpPost("Allocate")]
        public UniversityResponse Get()
        {
            _universityDAL.allocate();
            return new UniversityResponse
            {
                ID = 0,
                AllocatedAmount = 0
            };
        }
    }
}
