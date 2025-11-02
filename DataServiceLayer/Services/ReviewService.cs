using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Dtos;
using DataServiceLayer.DTOs;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataServiceLayer.Services
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

        public Rating? GetRatingById(string mediaId, Guid userId)
        {
            var db = new MediaDbContext(_connectionString);

            var rating = db.Ratings.FirstOrDefault(x => x.MediaId == mediaId && x.UserId == userId);

            return rating;
        }

        public async Task DeleteReview(Rating rating)
        {
            await using var db = new MediaDbContext(_connectionString);

            await using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                db.Ratings.Remove(rating);

                var existingReview = await db.Reviews
                    .FirstOrDefaultAsync(x => x.MediaId == rating.MediaId && x.UserId == rating.UserId);

                if (existingReview != null)
                {
                    db.Reviews.Remove(existingReview);

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

        public PaginatedResult<ReviewWithRating> GetByMediaId(string mediaId, int page, int pageSize)
        {
            var db = new MediaDbContext(_connectionString);

            var ratingsWithReviews = (
                from ratings in db.Ratings.Include(r => r.User)
                join reviews in db.Reviews
                on new { ratings.MediaId, ratings.UserId }
                equals new { reviews.MediaId, reviews.UserId } into ratingWithReview
                from reviews in ratingWithReview.DefaultIfEmpty()
                where ratings.MediaId == mediaId
                select new ReviewWithRating
                {
                    MediaId = ratings.MediaId,
                    UserId = ratings.User.Id,
                    Username = ratings.User.Username ?? "",
                    UserProfile = ratings.User.ProfileUrl ?? "",
                    Rating = ratings.Rating1 ?? 0,
                    Review = reviews.ReviewText,
                    CreatedAt = reviews != null ? reviews.CreatedAt : ratings.CreatedAt
                }
            );

            var ratingsWithReviewsPaginated = ratingsWithReviews
            .OrderBy(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            return new PaginatedResult<ReviewWithRating>
            {
                Items = ratingsWithReviewsPaginated,
                Total = ratingsWithReviews.Count()
            };
        }

        public int GetRatingsCount()
        {
            var db = new MediaDbContext(_connectionString);
            return db.Ratings.Count();
        }

    }
}