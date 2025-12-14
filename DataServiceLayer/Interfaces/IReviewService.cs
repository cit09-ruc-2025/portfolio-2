using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Dtos;
using DataServiceLayer.DTOs;
using DataServiceLayer.Models;

namespace DataServiceLayer.Interfaces
{
    public interface IReviewService
    {
        public Task UpsertReview(ReviewParam review);
        public Rating? GetRatingById(string mediaId, Guid userId);
        public Task DeleteReview(Rating rating);
        public PaginatedResult<ReviewWithRating> GetByMediaId(string mediaId, int page, int pageSize, Guid? userId);
        public PaginatedResult<ReviewWithRating> GetByUserId(Guid userId, int page, int pageSize);
        public bool HasUserReviewed(string mediaId, Guid userId);

    }
}