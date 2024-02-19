using BusinessLogic;
using DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BursaryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversityFundRequestController : ControllerBase
    {

        private readonly UniversityFundRequestBLL _UniversityFundRequestBLL;
         public UniversityFundRequestController(UniversityFundRequestBLL UniversityFundRequestBLL)
        {
            _UniversityFundRequestBLL = UniversityFundRequestBLL;
        }
        [HttpGet]
        public ActionResult<IEnumerable<University>> Get()
        {
            try
            {
                var universities = _UniversityFundRequestBLL.GetUniversities();
                return Ok(_UniversityFundRequestBLL.GetUniversities());
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
       
    }
}


public class newAllocation
{
    public int year { get; set; }
    
}

public class status
{
    public string statusName { get; set; }


    public string message { get; set; }

    public status(string status, string message)
    {
        this.statusName = status;
        this.message = message;
    }
}