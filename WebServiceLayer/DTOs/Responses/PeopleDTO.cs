namespace WebServiceLayer.DTOs.Responses
{
    public class PeopleDTO
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int? BirthDate { get; set; }

        public int? DeathDate { get; set; }

        public string? Description { get; set; }

        public decimal? NameRating { get; set; }

        public List<MediaDTO> KnownFor { get; set; } = new();
    }
}
