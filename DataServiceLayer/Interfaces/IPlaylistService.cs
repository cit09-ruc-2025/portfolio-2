using DataServiceLayer.Models;
using ListEntity = DataServiceLayer.Models.List;
using System;

namespace DataServiceLayer.Interfaces
{
    public interface IPlaylistService
    {

        List CreatePlaylist(Guid userID, string title, string? description);
        bool AddItemToPlaylist(Guid listId, string itemId, bool isMedia);
        bool RemoveItemFromPlaylist(Guid listId, string itemId, bool isMedia);
        bool DeletePlaylist(Guid listId);
        List<List> GetPlaylistsByUserId(Guid userId);

    }
}