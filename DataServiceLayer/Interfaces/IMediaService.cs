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
                public MediaDetailDTO? GetById(string id);
                public Task UpdateMediaRating(string id, MediaDbContext db);
                Task<(List<Media> Items, int TotalCount)> GetAllMedia(int page, int pageSize, MediaSortBy sortBy = MediaSortBy.ReleaseYear);
                public PaginatedResult<MediaList> GetByTitle(string title, int page, int pageSize);
                public (List<MediaPerson> PeopleMedia, int TotalCount) GetMediaForPeople(string peopleId, int page, int pageSize);

        }
}