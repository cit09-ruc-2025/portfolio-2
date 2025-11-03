using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataServiceLayer.Services
{
    public class GenreMediaService : IGenreMediaService
    {
        private readonly MediaDbContext _db;

        public GenreMediaService(string? connectionString)
        {
            _db = new MediaDbContext(connectionString);
        }

        public List<Media> GetMediaByGenre(string genreName)
        {
            return _db.Genres
                      .Where(g => g.Name == genreName)
                      .SelectMany(g => g.Media)
                      .ToList();
        }
    }
}
