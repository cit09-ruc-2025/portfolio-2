using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Models;

namespace DataServiceLayer.Interfaces
{
    public interface IReviewService
    {
        public Task UpsertReview(ReviewParam review);
        public Rating? GetRatingById(string mediaId, Guid userId);

        public Task DeleteReview(Rating rating);

    }
}