using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Dtos;
using DataServiceLayer.Models;

namespace DataServiceLayer.Interfaces
{
        public interface IMediaService
        {
                public Media? GetById(string id);
                public Task UpdateMediaRating(string id, MediaDbContext db);
                Task<(List<Media> Items, int TotalCount)> GetAllMedia(int page, int pageSize);
                public PaginatedResult<MediaList> GetByTitle(string title, int page, int pageSize);
        }
}