using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServiceLayer.Dtos
{
    public class ReviewParam
    {
        public Guid UserId { get; set; }

        public string MediaId { get; set; } = null!;

        public int Rating { get; set; }

        public string? Review { get; set; }

    }
}