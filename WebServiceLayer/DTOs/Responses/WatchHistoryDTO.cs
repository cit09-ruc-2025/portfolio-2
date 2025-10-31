namespace WebServiceLayer.DTOs.Responses
{
    public class WatchHistoryDTO
    {
        public Guid UserId { get; set; }
        public string MediaId { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
    }
}
