using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.UserService;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    var exceptionMessage = "Check your input values";
                    throw new ArgumentException(exceptionMessage);
                }

                var userId = await _userService.AddUser(user);
                if(userId == 0)
                {
                    throw new ArgumentException($"Username {user.UserName} already exists");
                }

                return Ok();
            }
            catch(ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }           
        }

        [HttpGet("CheckIfUsernameExists/{username}")]
        public async Task<IActionResult> CheckIfUsernameExists(string username)
        {
            try
            {
                var userExists = await _userService.CheckIfUsernameExists(username);
                return Ok(userExists);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
           
        }
    }
}
