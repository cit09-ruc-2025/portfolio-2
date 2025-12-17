using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Models;

namespace DataServiceLayer.Dtos
{
    public class MediaUserStatus
    {

        public bool IsReviewed { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsWatched { get; set; }
        public List<Guid> Playlists { get; set; } = new();

    }
}