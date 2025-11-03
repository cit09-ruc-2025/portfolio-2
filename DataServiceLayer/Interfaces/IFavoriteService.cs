using DataServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Interfaces
{
    public interface IFavoriteService
    {
        bool FavoritePerson(Guid userId, string peopleId);
        bool UnfavoritePerson(Guid userId, string peopleId);
        (List<FavoritePerson> FavoritePeople, int TotalCount) GetFavoritePeople(Guid userId, int pageNumber, int pageSize);
        bool FavoriteMedia(Guid userId, string mediaId);
        bool UnfavoriteMedia(Guid userId, string mediaId);
        (List<FavoriteMedia> FavoriteMedia, int TotalCount) GetFavoriteMedia(Guid userId, int pageNumber, int pageSize);

    }
}
