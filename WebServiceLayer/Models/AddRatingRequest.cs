using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceLayer.Models
{
    public class AddRatingRequest
    {
        [Required(ErrorMessage = "RATING_REQUIRED")]
        [Range(1, 10, ErrorMessage = "RATING_OUT_OF_RANGE")]
        public required int Rating { get; set; }
    }
}