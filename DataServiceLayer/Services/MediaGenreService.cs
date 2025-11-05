using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DataServiceLayer.Helpers;

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
            var result = _db.Media.Include(m => m.Genres)
                .Include(m => m.Titles)
                .Where(m => m.Genres.Any(g => g.Name.ToLower() == genreName.ToLower()))
                .ApplyPagination(pageNumber, pageSize)
                .ToList();

            return result;
        }
    }
}
