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
    public class SearchHistoryService : ISearchHistoryService
    {
        private readonly string? _connectionString;

        public SearchHistoryService(string? connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(string keyword, Guid userId)
        {
            var db = new MediaDbContext(_connectionString);

            var keywordExists = db.SearchHistories
                .FirstOrDefault(x =>
                    x.SearchText.ToLower() == keyword.ToLower()
                    && x.UserId == userId
            );

            if (keywordExists != null)
            {
                keywordExists.CreatedAt = DateTime.Now;
            }
            else
            {
                var newSearch = new SearchHistory
                {
                    UserId = userId,
                    SearchText = keyword
                };
                db.SearchHistories.Add(newSearch);
            }
            db.SaveChanges();
        }

        public void Delete(Guid userId)
        {
            var db = new MediaDbContext(_connectionString);

            db.SearchHistories.RemoveRange(
                db.SearchHistories.Where(x => x.UserId == userId)
            );

            db.SaveChanges();
        }

        public (List<SearchHistory>, int count) List(Guid userId, int page, int pageSize)
        {
            var db = new MediaDbContext(_connectionString);

            return db.SearchHistories
                 .Where(x => x.UserId == userId)
                 .OrderByDescending(x => x.CreatedAt)
                 .GetPaginatedResult(page, pageSize);

        }
    }
}