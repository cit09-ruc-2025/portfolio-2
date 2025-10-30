using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Models;

namespace DataServiceLayer.Interfaces
{
    public interface IRatingService
    {
        public Task UpsertRating(Rating rating);
        public Rating? GetById(Rating rating);

    }
}