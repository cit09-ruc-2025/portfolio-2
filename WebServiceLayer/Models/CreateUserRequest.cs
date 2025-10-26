using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceLayer.Models
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "USERNAME_REQUIRED")]
        [MaxLength(100, ErrorMessage = "USERNAME_MAX")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "USERNAME_INVALID")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "EMAIL_REQUIRED")]
        [EmailAddress(ErrorMessage = "INVALID_EMAIL")]
        [MaxLength(255, ErrorMessage = "EMAIL_MAX")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "PASSWORD_REQUIRED")]
        [MaxLength(12, ErrorMessage = "PASSWORD_MAX")]
        public required string Password { get; set; }

        [Url(ErrorMessage = "INVALID_URL")]
        public string? ProfileUrl { get; set; }
    }
}