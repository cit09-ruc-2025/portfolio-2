using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Dtos;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataServiceLayer.Services
{
    public class MediaService : IMediaService
    {
        private readonly string? _connectionString;

        public MediaService(string? connectionString)
        {
            _connectionString = connectionString;
        }
        public Media? GetById(string id)
        {
            var db = new MediaDbContext(_connectionString);
            var media = db.Media.FirstOrDefault(x => x.Id == id);
            return media;
        }

        public async Task UpdateMediaRating(string id, MediaDbContext db)
        {

            var media = db.Media.FirstOrDefault(x => x.Id == id);

            var newAverage = db.Ratings.Where(x => x.MediaId == id).Average(x => x.Rating1) ?? 0;

            media!.AverageRating = (decimal)newAverage;

            await db.SaveChangesAsync();

        }

        public async Task<(List<Media> Items, int TotalCount)> GetAllMedia(int page, int pageSize, MediaSortBy sortBy = MediaSortBy.ReleaseYear)
        {
            using var db = new MediaDbContext(_connectionString);

            var totalCount = await db.Media.CountAsync();

            IQueryable<Media> query = db.Media
                .Where(m => !m.EpisodeEpisodeMedia.Any() &&
                            (m.MediaType == "movie" || m.MediaType == "tvSeries"));

            switch (sortBy)
            {
                case MediaSortBy.ImdbAverageRating:
                    query = query.OrderByDescending(m => m.ImdbAverageRating ?? 0);
                    break;
                default:
                    query = query.OrderByDescending(m => m.ReleaseYear ?? 0);
                    break;
            }


            var pagedMedia = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var ids = pagedMedia.Select(m => m.Id).ToList();

            // Loading related data in separate queries to avoid the big join
            await db.Media
                .Where(m => ids.Contains(m.Id))
                .Include(m => m.Genres)
                .Include(m => m.DvdRelease)
                .Include(m => m.Titles)
                .LoadAsync();

            var items = pagedMedia.Select(m => db.Media.Local.First(x => x.Id == m.Id)).ToList();

            return (items, totalCount);
        }

        public PaginatedResult<MediaList> GetByTitle(string keyword, int page, int pageSize)
        {
            var db = new MediaDbContext(_connectionString);

            var query = db.Media
                .Where(x => x.Titles.Any(t => t.Title1.ToLower().Contains(keyword.ToLower())));

            var result = new PaginatedResult<MediaList>
            {
                Total = query.Count(),
                Items = query
                .OrderByDescending(t => t.ImdbAverageRating.HasValue)
                .ThenByDescending(t => t.ImdbAverageRating)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new MediaList
                {
                    MediaId = m.Id,
                    Title = m.Titles
                    .OrderBy(t => t.Ordering)
                    .Select(t => t.Title1)
                    .FirstOrDefault(),
                    Poster = m.Poster,
                    ReleaseYear = m.ReleaseYear ?? 0,
                    ImdbRating = m.ImdbAverageRating ?? 0

                })
                .ToList()
            };
            return result;
        }

        public MediaDetailDTO? GetMediaDetails(string mediaId)
        {
            using var db = new MediaDbContext(_connectionString);

            var media = db.Media
                          .Include(m => m.MediaPeople)
                              .ThenInclude(mp => mp.People)
                          .Include(m => m.MediaPeople)
                              .ThenInclude(mp => mp.Role)
                          .Include(m => m.Titles)
                          .FirstOrDefault(m => m.Id == mediaId);

            if (media == null) return null;

            var people = media.MediaPeople
                             .Select(mp => new PersonRoleDTO
                             {
                                 Name = mp.People.Name,
                                 Role = mp.Role.Name
                             })
                             .ToList();

            var primaryTitle = media.Titles.OrderBy(t => t.Ordering).FirstOrDefault()?.Title1;

            return new MediaDetailDTO
            {
                Id = media.Id,
                Title = primaryTitle,
                ReleaseYear = media.ReleaseYear,
                RuntimeMinutes = media.RuntimeMinutes,
                Plot = media.Plot,
                Poster = media.Poster,
                ImdbAverageRating = media.ImdbAverageRating,
                ImdbNumberOfVotes = media.ImdbNumberOfVotes,
                AverageRating = media.AverageRating,
                PeopleInvolved = people
            };
        }
    }
}