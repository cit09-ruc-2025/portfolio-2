namespace DataServiceLayer.Models;

public class MediaListItem
{
    public Guid ListId { get; set; }
    public UserList List { get; set; } = null!;

    public string MediaId { get; set; } = null!;
    public Media Media { get; set; } = null!;
}
