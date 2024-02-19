using BusinessLogic;
using DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BursaryManagementAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminBLL _adminBLL;

        public AdminController(AdminBLL adminBLL)
        {
            _adminBLL = adminBLL;
        }

        [Route("universityRequests")]
        [HttpGet]
        public ActionResult<IEnumerable<UniversityRequest>> Get()
        {
            try
            {
                return Ok(_adminBLL.GetAllUniversityRequests());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [Route("UniversityYearAllocations")]
        [HttpGet]
        public ActionResult<IEnumerable<AllocationDetails>> GetYearAllocations()
        {
            try
            {
                return Ok(_adminBLL.GetAllocationDetails());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        


        [Route("newUniversityRequest")]
        [HttpPost]
        public void Post(int universityID, decimal amount, string comment)
        {
            if (universityID == 0 || amount == 0 || comment == null)
            {
                BadRequest("Invalid input");
            }
            else
            {
                try
                {
                  Ok(_adminBLL.NewUniversityRequest(universityID, amount, comment));
                }
                catch (Exception ex)
                {
                 BadRequest(ex.Message);
                }
               
            }

        }

        [Route("updateUniversityRequest")]
        [HttpPut]
        public void Put(int requestId, int statusId )
        {
            if (requestId == 0 || statusId == 0 )
            {
                BadRequest("Invalid input");
            }
            else
            {
                try
                {
                    Ok(_adminBLL.UpdateUniversityRequest(requestId, statusId));
                }
                catch (Exception ex)
                {
                    BadRequest(ex.Message);
                }
            }
        }

    }
}
