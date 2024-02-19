using BusinessLogic;
using BusinessLogic.Models;
using BusinessLogic.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BursaryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserBLL _userManager;
        public AuthController(UserBLL userManager)
        {
            this._userManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]Register model)
        {
            if(ModelState.IsValid)
            {
                var result = await _userManager.RegisterUserAsync(model);
                if (result.isSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] Login model)
        {
            if(ModelState.IsValid)
            {
                var result = await _userManager.LoginUserAsync(model);
                if (result.isSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);

            }

            return BadRequest("Some properties are not valid");
        }



    }
}
