using System.ComponentModel.DataAnnotations;

namespace WebServiceLayer.DTOs.Requests
{
    public class CreatePlaylistDTO
    {
        [Required(ErrorMessage = "TITLE_REQUIRED")]
        [MaxLength(100, ErrorMessage = "TITLE_MAX_LENGTH_EXCEEDED")]
        [MinLength(3, ErrorMessage = "TITLE_MIN_LENGTH")]
        public string Title { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "DESCRIPTION_MAX_LENGTH_EXCEEDED")]
        public string? Description { get; set; }
    }
}
