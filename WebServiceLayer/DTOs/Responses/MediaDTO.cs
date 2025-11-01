namespace WebServiceLayer.DTOs.Responses
{
    public class MediaDTO
    {
        public string Id { get; set; } = null!;
        
        public string? DisplayTitle { get; set; }

        public int? ReleaseYear { get; set; }

        public int? EndYear { get; set; }

        public int? RuntimeMinutes { get; set; }

        public decimal? ImdbAverageRating { get; set; }

        public int? ImdbNumberOfVotes { get; set; }

        public string? Plot { get; set; }

        public string? Awards { get; set; }

        public string? AgeRating { get; set; }

        public string? Poster { get; set; }

        public string? Production { get; set; }

        public string? BoxOffice { get; set; }

        public string? WebsiteUrl { get; set; }

        public string? Metascore { get; set; }

        public decimal? AverageRating { get; set; }

    }
}
