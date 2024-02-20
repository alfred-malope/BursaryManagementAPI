using BusinessLogic.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace BursaryManagementAPI.Controllers
{
    /// <summary>
    /// The institution controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionController(UniversityDAL universityDAL) : ControllerBase
    {
        private readonly UniversityDAL _universityDAL = universityDAL;

        /// <summary>
        /// Gets the.
        /// </summary>        /// <returns>An UniversityResponse.</returns>
        [HttpPost("Allocate")]
        public UniversityResponse Allocate()
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