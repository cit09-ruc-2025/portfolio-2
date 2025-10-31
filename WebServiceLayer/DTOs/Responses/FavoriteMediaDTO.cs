namespace WebServiceLayer.DTOs.Responses
{
    public class FavoriteMediaDTO
    {
        public Guid UserId { get; set; }

        public string MediaId { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
