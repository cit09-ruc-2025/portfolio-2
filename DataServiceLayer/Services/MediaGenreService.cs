using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataServiceLayer.Services
{
    public class MediaGenreService : IMediaGenreService
    {
        private readonly MediaDbContext _db;

        public MediaGenreService(string? connectionString)
        {
            _db = new MediaDbContext(connectionString);
        }

        public List<Media> GetMediaByGenre(string genreName, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var skip = (pageNumber - 1) * pageSize;

            var result = _db.MediaGenres
            .Include(mg => mg.Media)
                .ThenInclude(m => m.Titles) 
            .Include(mg => mg.Genre)
            .Where(mg => mg.Genre.Name == genreName)
            .Select(mg => mg.Media)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            return result;
        }
    }
}
