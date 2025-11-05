using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Models;

namespace DataServiceLayer.DTOs
{
    public class ReviewWithRating
    {
        public Guid UserId { get; set; }
        public string? UserUrl { get; set; }
        public string Username { get; set; } = "";
        public string? UserProfile { get; set; }
        public string MediaId { get; set; } = null!;
        public string? MediaUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Review { get; set; } = "";
        public int Rating { get; set; }
    }
}