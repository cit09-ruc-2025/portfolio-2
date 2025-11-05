namespace DataServiceLayer.Models;

public class PeopleListItem
{
    public Guid ListId { get; set; }
    public UserList List { get; set; } = null!;

    public string PeopleId { get; set; } = null!;
    public Person People { get; set; } = null!;
}
