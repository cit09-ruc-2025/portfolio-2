using System.Collections.Generic;
using DataServiceLayer.Dtos;
using DataServiceLayer.Models;

namespace DataServiceLayer.Dtos
{
    public class MediaDetailDTO
    {
        public string Id { get; set; } = null!;
        public string? Title { get; set; }
        public int? ReleaseYear { get; set; }
        public int? EndYear { get; set; }
        public int? RuntimeMinutes { get; set; }
        public string? Plot { get; set; }
        public string? Poster { get; set; }
        public decimal? ImdbAverageRating { get; set; }
        public int? ImdbNumberOfVotes { get; set; }
        public decimal? AverageRating { get; set; }
        public List<string> Genres { get; set; } = new();
        public List<string> Titles { get; set; } = new();
        public string? BoxOffice { get; set; }
        public string? Production { get; set; }
        public string? WebsiteUrl { get; set; }
        public List<string> Languages { get; set; } = new();
        public string? AgeRating { get; set; }
        public bool? HasEpisodes { get; set; }
    }
}
