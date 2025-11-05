using System.Collections.Generic;
using DataServiceLayer.Dtos;

namespace DataServiceLayer.Dtos
{
    public class MediaDetailDTO
    {
        public string Id { get; set; } = null!;
        public string? Title { get; set; }
        public int? ReleaseYear { get; set; }
        public int? RuntimeMinutes { get; set; }
        public string? Plot { get; set; }
        public string? Poster { get; set; }
        public decimal? ImdbAverageRating { get; set; }
        public int? ImdbNumberOfVotes { get; set; }
        public decimal? AverageRating { get; set; }

        public List<PersonRoleDTO> PeopleInvolved { get; set; } = new();
    }
}
