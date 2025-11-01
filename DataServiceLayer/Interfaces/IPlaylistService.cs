using DataServiceLayer.Models;
using System;

namespace DataServiceLayer.Interfaces
{
    public interface IPlaylistService
    {

        Playlist CreatePlaylist(Guid userID, string title, string? description);
        bool AddMediaToPlaylist(Guid playlistId, string mediaId);
        bool RemoveMediaFromPlaylist(Guid playlistId, string mediaId);
        bool DeletePlaylist(Guid playlistId);
        List<Playlist> GetPlaylistsByUserId(Guid userId);
        Playlist? GetPlaylistById(Guid playlistId);

    }
}