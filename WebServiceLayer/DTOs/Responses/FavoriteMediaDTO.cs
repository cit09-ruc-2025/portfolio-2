namespace WebServiceLayer.DTOs.Responses
{
    public class FavoriteMediaDTO
    {
        public Guid UserId { get; set; }

        public string MediaId { get; set; } = null!;

        public string Title { get; set; }

        public decimal? ImdbRating { get; set; }

        public int? ReleaseYear { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool HasEpisodes { get; set; }
    }
}
