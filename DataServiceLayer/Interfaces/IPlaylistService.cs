using DataServiceLayer.Models;
using ListEntity = DataServiceLayer.Models.UserList;
using System;

namespace DataServiceLayer.Interfaces
{
    public interface IPlaylistService
    {

        UserList CreatePlaylist(Guid userID, string title, string? description);
        bool AddItemToPlaylist(Guid listId, string itemId, bool isMedia);
        bool RemoveItemFromPlaylist(Guid listId, string itemId, bool isMedia);
        bool DeletePlaylist(Guid listId);
        List<UserList> GetPlaylistsByUserId(Guid userId);

    }
}