using DataServiceLayer.Models;
using DataServiceLayer.Interfaces;
using System.Collections.Generic;
using System.Linq;

public class GenreMediaService : IGenreMediaService
{
    private readonly MediaDbContext _db;

    public GenreMediaService(MediaDbContext db)
    {
        _db = db;
    }

    public List<Media> GetMediaByGenre(string genreName)
    {
        return _db.Media
                  .Where(m => m.Genres.Any(g => g.Name == genreName))
                  .ToList();
    }
}
