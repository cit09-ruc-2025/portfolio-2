using DataServiceLayer.Models;
using ListEntity = DataServiceLayer.Models.UserList;
using System;

namespace DataServiceLayer.Interfaces
{
    public interface IPlaylistService
    {

        UserList CreatePlaylist(Guid userID, string title, string? description);
        bool AddItemToPlaylist(Guid listId, string itemId, bool isMedia, Guid currentUserId);
        bool RemoveItemFromPlaylist(Guid listId, string itemId, bool isMedia, Guid currentUserId);
        bool DeletePlaylist(Guid listId, Guid currentUserId);
        List<UserList> GetPlaylistsByUserId(Guid userId);
        UserList? GetPlaylistByPlaylistId(Guid playlistId);

    }
}