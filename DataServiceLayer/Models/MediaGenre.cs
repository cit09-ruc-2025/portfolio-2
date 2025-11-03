using System;

namespace DataServiceLayer.Models
{
    public class MediaGenre
    {
        public string MediaId { get; set; } = null!;
        public Media Media { get; set; } = null!;

        public Guid GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
    }
}
