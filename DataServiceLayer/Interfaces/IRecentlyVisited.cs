using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Models;

namespace DataServiceLayer.Interfaces
{
    public interface IRecentlyVisited
    {
        public void Add(string? mediaId, string? peopleId, Guid userId);
        public (List<RecentlyViewed>, int count) List(Guid userId, int page, int pageSize);
    }
}