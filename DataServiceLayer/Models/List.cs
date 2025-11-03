namespace DataServiceLayer.Models;
public class List
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public User User { get; set; } = null!;
    public ICollection<MediaListItem> MediaListItems { get; set; } = new List<MediaListItem>();
    public ICollection<PeopleListItem> PeopleListItems { get; set; } = new List<PeopleListItem>();
}