using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceLayer.DTOs.Responses
{
    public class PeopleMediaDTO
    {
        public string MediaId { get; set; } = null!;
        public string Title { get; set; } = "";
        public int ReleaseYear { get; set; }
        public decimal? ImdbAverageRating { get; set; }
    }
}