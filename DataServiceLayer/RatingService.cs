using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataServiceLayer
{
    public class RatingService : IRatingService
    {
        private readonly string? _connectionString;

        public RatingService(string? connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task UpsertRating(Rating rating)
        {
            await using var db = new MediaDbContext(_connectionString);

            await using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                var existingRating = await db.Ratings
        .FirstOrDefaultAsync(x => x.MediaId == rating.MediaId && x.UserId == rating.UserId);


                if (existingRating == null)
                {
                    db.Ratings.Add(rating);

                }
                else
                {

                    existingRating.Rating1 = rating.Rating1;

                }


                await db.SaveChangesAsync();

                var mediaService = new MediaService(_connectionString);

                await mediaService.UpdateMediaRating(rating.MediaId, db);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        public Rating? GetById(Rating ratingParam)
        {
            var db = new MediaDbContext(_connectionString);

            var rating = db.Ratings.FirstOrDefault(x => x.MediaId == ratingParam.MediaId && x.UserId == ratingParam.UserId);

            return rating;

        }


    }
}