using DataServiceLayer.Models;
using DataServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataServiceLayer.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly MediaDbContext _db;

        public PlaylistService(string? connectionString)
        {
            _db = new MediaDbContext(connectionString);
        }

        public UserList CreatePlaylist(Guid currentUserId, string title, string? description)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.Id == currentUserId);
            if (existingUser == null) throw new Exception("User does not exist.");

            var playlist = new UserList
            {
                Id = Guid.NewGuid(),
                UserId = currentUserId,
                Title = title,
                Description = description,
                CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
                UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
            };

            _db.Lists.Add(playlist);
            _db.SaveChanges();
            return playlist;
        }

        public bool AddItemToPlaylist(Guid listId, string itemId, bool isMedia, Guid currentUserId)
        {
            var list = _db.Lists.Include(l => l.Media)
                                .Include(l => l.People)
                                .FirstOrDefault(l => l.Id == listId);
            if (list == null || list.UserId != currentUserId) return false;

            list.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

            if (isMedia)
            {
                var media = _db.Media.Find(itemId);
                if (media == null || list.Media.Any(m => m.Id == itemId)) return false;
                list.Media.Add(media);
            }
            else
            {
                var people = _db.People.Find(itemId);
                if (people == null || list.People.Any(p => p.Id == itemId)) return false;
                list.People.Add(people);
            }

            _db.SaveChanges();
            return true;
        }

        public bool RemoveItemFromPlaylist(Guid listId, string itemId, bool isMedia, Guid currentUserId)
        {
            var list = _db.Lists.Include(l => l.Media)
                                .Include(l => l.People)
                                .FirstOrDefault(l => l.Id == listId && l.UserId == currentUserId);
            if (list == null) return false;

            if (isMedia)
            {
                var mediaItem = _db.Media.FirstOrDefault(m => m.Id == itemId);
                if (mediaItem == null) return false;
                list.Media.Remove(mediaItem);
            }
            else
            {
                var personItem = _db.People.FirstOrDefault(p => p.Id == itemId);
                if (personItem == null) return false;
                list.People.Remove(personItem);
            }

            list.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            _db.SaveChanges();
            return true;
        }

        public bool DeletePlaylist(Guid listId, Guid currentUserId)
        {
            var list = _db.Lists.FirstOrDefault(l => l.Id == listId && l.UserId == currentUserId);
            if (list == null) return false;

            _db.Lists.Remove(list);
            _db.SaveChanges();
            return true;
        }

        public List<UserList> GetPlaylistsByUserId(Guid userId)
        {
            return _db.Lists
                .Where(p => p.UserId == userId)
                .Include(p => p.User)
                .Include(p => p.People)
                .Include(p => p.Media)
                .ThenInclude(m => m.Titles)
                .ToList();
        }

        public UserList? GetPlaylistByPlaylistId(Guid playlistId)
        {
            return _db.Lists
                .Where(p => p.Id == playlistId)
                .Include(p => p.User)
                .Include(p => p.People)
                .Include(p => p.Media)
                .ThenInclude(m => m.Titles)
                .FirstOrDefault();
        }
    }
}
