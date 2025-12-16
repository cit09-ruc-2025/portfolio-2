using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceLayer.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public required string Username { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
    }


}