namespace DataServiceLayer.Models;
public class Playlist
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public User User { get; set; } = null!;

    public ICollection<PlaylistItem> PlaylistItems { get; set; } = new List<PlaylistItem>();
}