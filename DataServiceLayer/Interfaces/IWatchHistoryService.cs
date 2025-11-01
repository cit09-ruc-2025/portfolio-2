using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Interfaces
{
    public interface IWatchHistoryService
    {
        List<Models.WatchHistory> GetWatchHistory(Guid userGuid, int pageNumber, int pageSize);
        bool AddToWatched(string mediaId, Guid userGuid);
        bool RemoveFromWatched(string mediaId, Guid userGuid);
    }
}
