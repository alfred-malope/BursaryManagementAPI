using BursaryManagementAPI.Models.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using BursaryManagementAPI.Models.DTO;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BursaryManagementAPI.Controllers


{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionDetailsController : ControllerBase
            {
         private readonly ILogger<InstitutionDetailsController> _logger;
         private readonly UniversityDAO _universityDAO;
        private readonly DBManager _dbManager;


        //using dependency injects to get required ocjects
        public InstitutionDetailsController(ILogger<InstitutionDetailsController> logger, UniversityDAO universityDAO, DBManager dBManager)
        {
            _dbManager = dBManager;
            _universityDAO = universityDAO;
            _logger = logger;
        }
        // GET: api/<InstitutionDetailsController>
        [Route("api/AllUniveritydetails")]
        [HttpGet]
        public List<University> Get()
        {
            
            return _universityDAO.GetUniversities();
        }

        // GET api/<InstitutionDetailsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<InstitutionDetailsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
           
               _dbManager.allocate();
            
            
            
        }

        // PUT api/<InstitutionDetailsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InstitutionDetailsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
