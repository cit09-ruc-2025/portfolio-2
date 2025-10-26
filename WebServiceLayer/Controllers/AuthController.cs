using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models;
using WebServiceLayer.Utils;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult Login(LoginRequest loginData)
        {
            var user = _userService.GetUserByUsername(loginData.Username);

            if (user == null)
            {
                return BadRequest(new { message = "INVALID_CREDENTIALS" });
            }

            var PasswordHasher = new PasswordHasher();

            if (!PasswordHasher.VerifyPassword(loginData.Password, user.HashedPassword))
            {
                return BadRequest(new { message = "INVALID_CREDENTIALS" });
            }

            return Ok();

        }



    }
}