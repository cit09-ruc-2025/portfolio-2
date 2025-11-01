using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataServiceLayer
{
    public class ReviewService : IReviewService
    {
        private readonly string? _connectionString;

        public ReviewService(string? connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task UpsertReview(ReviewParam review)
        {
            await using var db = new MediaDbContext(_connectionString);

            await using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                var existingRating = await db.Ratings
                    .FirstOrDefaultAsync(x => x.MediaId == review.MediaId && x.UserId == review.UserId);

                if (existingRating == null)
                {
                    var newRating = new Rating
                    {
                        MediaId = review.MediaId,
                        UserId = review.UserId,
                        Rating1 = review.Rating
                    };
                    db.Ratings.Add(newRating);
                }
                else
                {
                    existingRating.Rating1 = review.Rating;
                }

                var existingReview = await db.Reviews
                                    .FirstOrDefaultAsync(x => x.MediaId == review.MediaId && x.UserId == review.UserId);

                if (!string.IsNullOrWhiteSpace(review.Review))
                {
                    if (existingReview == null)
                    {
                        var newReview = new Review
                        {
                            MediaId = review.MediaId,
                            UserId = review.UserId,
                            ReviewText = review.Review
                        };
                        db.Reviews.Add(newReview);
                    }
                    else
                    {

                        existingReview.ReviewText = review.Review;
                    }
                }
                else
                {
                    if (existingReview != null)
                    {
                        db.Reviews.Remove(existingReview);
                    }

                }

                await db.SaveChangesAsync();

                var mediaService = new MediaService(_connectionString);

                await mediaService.UpdateMediaRating(review.MediaId, db);

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