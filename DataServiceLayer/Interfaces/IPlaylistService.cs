using DataServiceLayer.Models;
using System;

namespace DataServiceLayer.Interfaces
{
    public interface IPlaylistService
    {

        List CreatePlaylist(Guid userID, string title, string? description);
        bool AddMediaToPlaylist(Guid playlistId, string mediaId);
        bool RemoveMediaFromPlaylist(Guid playlistId, string mediaId);
        bool DeletePlaylist(Guid playlistId);
        List<List> GetPlaylistsByUserId(Guid userId);
        List? GetPlaylistById(Guid playlistId);

    }
}