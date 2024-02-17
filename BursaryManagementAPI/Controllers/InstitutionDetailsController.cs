using example.ApplicationModels;
using example.DataAccess;
using example.DataAccess.models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace example.Controllers


{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionDetailsController : ControllerBase
            {
         private readonly ILogger<InstitutionDetailsController> _logger;

        public InstitutionDetailsController(ILogger<InstitutionDetailsController> logger)
        {
            _logger = logger;
        }
        // GET: api/<InstitutionDetailsController>
        [Route("api/AllUniveritydetails")]
        [HttpGet]
        public List<University> Get()
        {
            
            return UniversityADO.getUniversities();
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
           
               new DBManager().allocate();
            
            
            
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
