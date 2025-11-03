using DataServiceLayer.Models;
using DataServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;

namespace DataServiceLayer.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly MediaDbContext _db;

        public PlaylistService(string? connectionString)
        {
            _db = new MediaDbContext(connectionString);
        }

        public List CreatePlaylist(Guid userID, string title, string? description)
        {
            var playlist = new List
            {
                Id = Guid.NewGuid(),
                UserId = userID,
                Title = title,
                Description = description,
                CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
                UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
            };

            _db.Playlists.Add(playlist);
            _db.SaveChanges();
            return playlist;
        }

        public bool AddMediaToPlaylist(Guid playlistId, string mediaId)
        {
            var playlist = _db.Playlists
                .Where(p => p.Id == playlistId)
                .FirstOrDefault();
            if (playlist != null)
            {
                var playlistItem = new PlaylistItem
                {
                    Id = Guid.NewGuid(),
                    MediaId = mediaId,
                    PlaylistId = playlistId
                };
                _db.PlaylistItems.Add(playlistItem);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RemoveMediaFromPlaylist(Guid playlistId, string mediaId)
        {
            var item = _db.PlaylistItems
            .FirstOrDefault(p => p.PlaylistId == playlistId && p.MediaId == mediaId);
            if (item != null)
            {
                _db.PlaylistItems.Remove(item);

                var playlist = _db.Playlists.First(p => p.Id == playlistId);
                playlist.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
                return true;
            }
            return false;
        }

        public bool DeletePlaylist(Guid playlistId)
        {
            var playlist = _db.Playlists
                .FirstOrDefault(p => p.Id == playlistId);
            if (playlist != null)
            {
                _db.Playlists.Remove(playlist);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public List<List> GetPlaylistsByUserId(Guid userId)
        {
            return _db.Playlists
            .Where(p => p.UserId == userId)
            .ToList();
        }

        public List? GetPlaylistById(Guid playlistId)
        {
            return _db.Playlists
            .FirstOrDefault(p => p.Id == playlistId);
        }

    }
}