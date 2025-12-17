using System.Text.Json.Serialization;

namespace WebServiceLayer.DTOs.Responses
{
    public class WatchHistoryDTO
    {
        public Guid UserId { get; set; }
        public string MediaId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public decimal? ImdbAverageRating { get; set; }
        public int? ReleaseYear { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool HasEpisodes { get; set; }

    }
}
