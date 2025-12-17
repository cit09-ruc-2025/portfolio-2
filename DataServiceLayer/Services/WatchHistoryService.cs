using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServiceLayer.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DataServiceLayer.Services
{
    public class WatchHistoryService : IWatchHistoryService
    {
        private readonly string? _connectionString;

        public WatchHistoryService(string? connectionString)
        {
            _connectionString = connectionString;
        }
        private MediaDbContext CreateContext() => new(_connectionString);
        public (List<WatchHistory> WatchHistory, int TotalCount) GetWatchHistory(Guid userGuid, int pageNumber, int pageSize)
        {
            var context = CreateContext();

            var baseQuery = context.WatchHistories.Where(wh => wh.UserId == userGuid).Include(x => x.Media).ThenInclude(x => x.Titles).Include(x => x.Media.EpisodeSeriesMedia).OrderBy(x => x.CreatedAt);
            return baseQuery.GetPaginatedResult(pageNumber, pageSize);
        }

        public bool AddToWatched(string mediaId, Guid userGuid)
        {
            var context = CreateContext();

            var exists = context.WatchHistories.Any(wh => wh.UserId == userGuid && wh.MediaId == mediaId);

            if (exists) return false;

            context.WatchHistories.Add(new WatchHistory
            {
                MediaId = mediaId,
                UserId = userGuid,
                CreatedAt = DateTime.Now
            });

            context.SaveChanges();
            return true;
        }

        public bool RemoveFromWatched(string mediaId, Guid userGuid)
        {
            var context = CreateContext();

            var toBeRemoved = context.WatchHistories.FirstOrDefault(wh => wh.UserId == userGuid && wh.MediaId == mediaId);

            if (toBeRemoved == null) return false;

            context.WatchHistories.Remove(toBeRemoved);
            context.SaveChanges();

            return true;
        }
        public bool IsWatched(string mediaId, Guid userId)
        {
            var db = new MediaDbContext(_connectionString);

            return db.WatchHistories.Any(x => x.MediaId == mediaId && x.UserId == userId);
        }
    }
}
