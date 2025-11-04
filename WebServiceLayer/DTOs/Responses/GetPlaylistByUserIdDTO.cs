namespace WebServiceLayer.Models
{
    public class GetPlaylistByUserIdDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<MediaListItemDTO> MediaListItems { get; set; } = new();
        public List<PeopleListItemDTO> PeopleListItems { get; set; } = new();
    }

    public class MediaListItemDTO
    {
        public string MediaId { get; set; } = null!;
    }

    public class PeopleListItemDTO
    {
        public string PeopleId { get; set; } = null!;
    }
}
