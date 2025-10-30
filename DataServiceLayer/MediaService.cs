using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;

namespace DataServiceLayer
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

    }
}