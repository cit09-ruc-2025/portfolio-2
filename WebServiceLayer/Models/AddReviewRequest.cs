using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceLayer.Models
{
    public class AddReviewRequest
    {
        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 10, ErrorMessage = "Rating should be between 1 and 10")]
        public required int Rating { get; set; }

        [MaxLength(250, ErrorMessage = "Review cannot exceed 250 characters")]
        public string? Review { get; set; }
    }
}