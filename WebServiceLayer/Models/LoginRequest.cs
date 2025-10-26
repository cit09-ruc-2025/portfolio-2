using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceLayer.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "USERNAME_REQUIRED")]
        public required string Username { get; set; }


        [Required(ErrorMessage = "PASSWORD_REQUIRED")]
        public required string Password { get; set; }
    }


}