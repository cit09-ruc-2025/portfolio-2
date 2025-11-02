using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceLayer.Models
{
    public class GetMediaByGenre
    {
        public string Id { get; set; } = null!;
        public string? Title { get; set; }
        public int? ReleaseYear { get; set; }
        public string? Poster { get; set; }
        public decimal? AverageRating { get; set; }
    }
}