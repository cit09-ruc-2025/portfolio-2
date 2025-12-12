using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServiceLayer.Dtos
{
    public class EpisodeDTO
    {
        public string Title { get; set; } = "";
        public string Id { get; set; } = null!;
        public string? Plot { get; set; } = "";
        public decimal? ImdbRating { get; set; }
        public int? EpisodeNumber { get; set; }


    }
}