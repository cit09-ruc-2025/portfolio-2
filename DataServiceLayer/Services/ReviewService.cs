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

        public PaginatedResult<ReviewWithRating> GetByMediaId(string mediaId, int page, int pageSize, Guid? userId)
        {
            var db = new MediaDbContext(_connectionString);

            var query = db.Ratings
                        .Where(r => r.MediaId == mediaId)
                        .Include(r => r.User)
                        .Include(r => r.Review);

            var total = query.Count();

            var items = query
                        .OrderByDescending(r => r.Review != null ? r.Review.CreatedAt : r.CreatedAt)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .Select(r => new ReviewWithRating
                        {
                            MediaId = r.MediaId,
                            UserId = r.User.Id,
                            Username = r.User.Username ?? "",
                            UserProfile = r.User.ProfileUrl ?? "",
                            Rating = r.Rating1 ?? 0,
                            Review = r.Review != null ? r.Review.ReviewText : "",
                            CreatedAt = r.Review != null ? r.Review.CreatedAt : r.CreatedAt
                        })
                        .ToList();

            if (userId.HasValue)
            {
                var userReview = items.FirstOrDefault(x => x.UserId == userId.Value);
                if (userReview != null)
                {
                    items.Remove(userReview);
                    items.Insert(0, userReview);
                }
            }

            var result = new PaginatedResult<ReviewWithRating>
            {
                Total = total,
                Items = items
            };

            return result;
        }

        public PaginatedResult<ReviewWithRating> GetByUserId(Guid userId, int page, int pageSize)
        {
            var db = new MediaDbContext(_connectionString);

            var query = from ratings in db.Ratings
                        join user in db.Users on ratings.UserId equals user.Id
                        join reviews in db.Reviews
                        on new { ratings.MediaId, ratings.UserId }
                        equals new { reviews.MediaId, reviews.UserId } into ratingWithReview
                        from reviews in ratingWithReview.DefaultIfEmpty()
                        where ratings.UserId == userId
                        select new { ratings, reviews, user };

            var result = new PaginatedResult<ReviewWithRating>
            {
                Total = query.Count(),
                Items = query
                    .OrderBy(x => x.reviews != null ? x.reviews.CreatedAt : x.ratings.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new ReviewWithRating
                    {
                        MediaId = x.ratings.MediaId,
                        UserId = x.user.Id,
                        Username = x.user.Username ?? "",
                        UserProfile = x.user.ProfileUrl ?? "",
                        Rating = x.ratings.Rating1 ?? 0,
                        Review = x.reviews != null ? x.reviews.ReviewText : null,
                        CreatedAt = x.reviews != null ? x.reviews.CreatedAt : x.ratings.CreatedAt
                    })
                    .ToList()
            };

            return result;
        }

        public bool HasUserReviewed(string mediaId, Guid userId)
        {
            var db = new MediaDbContext(_connectionString);

            return db.Ratings.Any(x => x.MediaId == mediaId && x.UserId == userId);
        }

    }
}