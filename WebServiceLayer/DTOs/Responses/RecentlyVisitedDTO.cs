using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceLayer.DTOs.Responses
{
    public class RecentlyVisitedDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? MediaId { get; set; } = "";
        public string? MediaUrl { get; set; } = "";
        public string? PeopleId { get; set; }
        public string? PeopleUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public MediaDTO? Media { get; set; }
        public PeopleDTO? People { get; set; }

    }
}