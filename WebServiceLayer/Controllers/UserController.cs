using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUserRequest user)
        {
            var validationErrors = new Dictionary<string, string>();

            bool emailExists = _userService.GetUserByEmail(user.Email) != null;

            if (emailExists)
            {
                validationErrors.Add("email", "EMAIL_EXISTS");
            }

            bool usernameExists = _userService.GetUserByUsername(user.Username) != null;

            if (usernameExists)
            {
                validationErrors.Add("username", "USERNAME_EXISTS");

            }

            if (validationErrors.Any())
            {
                return BadRequest(new { errors = validationErrors });
            }

            var newUser = new User
            {
                Email = user.Email,
                Username = user.Username,
                HashedPassword = user.Password,
                ProfileUrl = user.ProfileUrl
            };

            _userService.CreateUser(newUser);

            return Created();


        }
    }
}