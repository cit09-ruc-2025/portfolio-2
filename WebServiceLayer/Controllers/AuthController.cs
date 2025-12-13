using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataServiceLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebServiceLayer.Models;
using WebServiceLayer.Utils;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login(LoginRequest loginData)
        {
            var user = _userService.GetUserByUsername(loginData.Username);

            if (user == null)
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

            var PasswordHasher = new PasswordHasher();

            if (!PasswordHasher.VerifyPassword(loginData.Password, user.HashedPassword))
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

            var claims = new List<Claim>
            {
                new Claim("id",user.Id.ToString())
            };

            var secret = _configuration.GetSection("Auth:Secret").Value;
            var expiresIn = _configuration.GetSection("Auth:ExpiresInDays").Value;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(int.Parse(expiresIn)),
            signingCredentials: creds
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                username = user.Username,
                token = jwtToken
            });

        }
    }
}