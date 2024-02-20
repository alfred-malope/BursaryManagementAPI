﻿using BusinessLogic;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;

namespace BursaryManagementAPI.Controllers
{
    /// <summary>
    /// The auth controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserBLL userManager) : ControllerBase
    {
        private readonly UserBLL _userManager = userManager;

        /// <summary>
        /// Logins the user and issues bearer token.
        /// </summary>
        /// <param name="model">The Login model.</param>
        /// <returns>A Task.</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] Login model)
        {
            if (ModelState.IsValid)
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

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] Register model)
        {
            if (ModelState.IsValid)
            {
                var result = _userManager.ProcessRegistration(model);
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