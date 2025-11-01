namespace DataServiceLayer.Models;

public class PlaylistItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PlaylistId { get; set; }
    public string MediaId { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public Playlist Playlist { get; set; } = null!;
    public Media Media { get; set; } = null!;
}
