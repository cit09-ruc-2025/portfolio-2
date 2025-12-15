namespace WebServiceLayer.DTOs.Responses
{
    public class FavoritePeopleDTO
    {
        public Guid UserId { get; set; }

        public string PeopleId { get; set; } = null!;
        public string Name { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
