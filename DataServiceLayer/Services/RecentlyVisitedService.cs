using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Dtos;
using DataServiceLayer.Helpers;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;

namespace DataServiceLayer.Services
{
    public class RecentlyVisitedService : IRecentlyVisited
    {
        private readonly string? _connectionString;

        public RecentlyVisitedService(string? connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(string? mediaId, string? peopleId, Guid userId)
        {
            var db = new MediaDbContext(_connectionString);

            var newRecord = new RecentlyViewed
            {
                UserId = userId,
                PeopleId = string.IsNullOrWhiteSpace(peopleId) ? null : peopleId,
                MediaId = string.IsNullOrWhiteSpace(mediaId) ? null : mediaId
            };

            db.RecentlyVieweds.Add(newRecord);

            db.SaveChanges();
        }

        public (List<RecentlyViewed>, int count) List(Guid userId, int page, int pageSize)
        {
            var db = new MediaDbContext(_connectionString);

            return db.RecentlyVieweds.Where(x => x.UserId == userId).GetPaginatedResult(page, pageSize);
        }

    }
}