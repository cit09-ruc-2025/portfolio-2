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

        public List<Media> GetMediaByGenre(Guid genreGuid, int pageNumber = 1, int pageSize = 10)
        {
            var result = _db.Genres.Where(g => g.Id == genreGuid)
                .Include(g => g.Media)
                .SelectMany(g => g.Media)
                .Include(m => m.Titles)
                .Where(m => !m.EpisodeEpisodeMedia.Any())
                .OrderByDescending(m => m.ImdbNumberOfVotes ?? 0)
                .ThenByDescending(m => m.ImdbAverageRating ?? 0)
                .ThenByDescending(m => m.Id)
                .ApplyPagination(pageNumber, pageSize)
                .ToList();

            return result;
        }
    }
}
