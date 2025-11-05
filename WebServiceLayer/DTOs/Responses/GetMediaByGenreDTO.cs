using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceLayer.Models
{
    public class GetMediaByGenreDTO
    {
        public string MediaId { get; set; } = null!;
        public string? Title { get; set; }
        public int? ReleaseYear { get; set; }
        public string? Poster { get; set; }
        public decimal? AverageRating { get; set; }
        public string? MediaUrl { get; set; }
    }
}