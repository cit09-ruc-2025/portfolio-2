namespace DataServiceLayer.Models;

    public class PlaylistItem
{
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PlaylistId { get; set; }
        public string MediaId { get; set; } = null!;
    }
