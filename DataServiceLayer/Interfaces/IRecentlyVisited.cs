using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServiceLayer.Interfaces
{
    public interface IRecentlyVisited
    {
        public void Add(string? mediaId, string? peopleId, Guid userId);

    }
}