using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServiceLayer.Dtos
{
    public class MediaList
    {
        public string MediaId { get; set; } = "";
        public string MediaUrl { get; set; } = "";
        public string Title { get; set; } = "";
        public string Poster { get; set; } = "";
        public int ReleaseYear { get; set; }
        public decimal ImdbRating { get; set; }
    }
}