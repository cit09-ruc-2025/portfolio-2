using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Models;

namespace DataServiceLayer.Interfaces
{
    public interface IMediaService
    {
        public Media? GetById(string id);

        public Task UpdateMediaRating(string id, MediaDbContext db);

    }
}