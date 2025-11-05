using System;
using System.ComponentModel.DataAnnotations;

namespace WebServiceLayer.Models
{
    public class CreatePlaylistDTO
    {
        [Required(ErrorMessage = "USERID_REQUIRED")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "TITLE_REQUIRED")]
        [MaxLength(100, ErrorMessage = "TITLE_MAX_LENGTH_EXCEEDED")]
        [MinLength(3, ErrorMessage = "TITLE_MIN_LENGTH")]
        public string Title { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "DESCRIPTION_MAX_LENGTH_EXCEEDED")]
        public string? Description { get; set; }
    }
}

