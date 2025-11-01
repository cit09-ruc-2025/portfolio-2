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
        public Rating? GetById(Rating rating);

    }
}