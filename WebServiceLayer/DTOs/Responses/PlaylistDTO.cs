namespace WebServiceLayer.DTOs.Responses
{
    public class PlaylistDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public UserDTO User { get; set; } = null!;
        public List<string> MediaIds { get; set; } = new();
        public List<string> PeopleIds { get; set; } = new();
    }

}
