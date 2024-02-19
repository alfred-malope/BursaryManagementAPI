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

        // GET: api/<UniversityFundRequestController>
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
       

        // GET api/<UniversityFundRequestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UniversityFundRequestController>
        [HttpPost]
        public ActionResult Post([FromBody] newAllocation value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _UniversityFundRequestBLL.Allocate();
                return Ok(new status("Succesful","Budget allocated in the all the institutions"));
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new status("Unsuccessful", "Error: "+ex.Message));
            }
        }

        // PUT api/<UniversityFundRequestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UniversityFundRequestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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