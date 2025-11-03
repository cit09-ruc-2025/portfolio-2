using DataServiceLayer.Models;

namespace WebServiceLayer.DTOs.Responses
{
    public class MediaPeopleDTO
    {
        public string MediaId { get; set; } = null!;

        public string PeopleId { get; set; } = null!;

        public Guid RoleId { get; set; }

        public bool? KnownFor { get; set; }

        public int? Ordering { get; set; }

        public string? Description { get; set; }

        public string? PersonName { get; set; }

        public string? RoleName { get; set; }
    }
}
