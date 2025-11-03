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

        public List<Media> GetMediaByGenre(string genreName, int pageNumber = 0, int pageSize = 10)
        {
            if (pageNumber < 0) pageNumber = 0; // page 0 is the first page
            if (pageSize < 1) pageSize = 10;

            return _db.Genres
                      .Where(g => g.Name == genreName)
                      .SelectMany(g => g.Media)
                      .Skip(pageNumber * pageSize) // 0-based page
                      .Take(pageSize)
                      .ToList();
        }

    }
}
