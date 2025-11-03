using System;

namespace WebServiceLayer.Models
{
    public class GetSimilarMoviesDTO
    {
        public string MediaId { get; set; } = null!;
        public string? Title { get; set; }
        public int? ReleaseYear { get; set; }
        public string? Poster { get; set; }
        public decimal? AverageRating { get; set; }
        public string? Plot { get; set; } 
    }
}
