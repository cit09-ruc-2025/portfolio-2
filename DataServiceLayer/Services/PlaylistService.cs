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

        public UserList CreatePlaylist(Guid currentUserId, string title, string? description, bool isPublic)
        {
            var playlist = new UserList
            {
                UserId = currentUserId,
                Title = title,
                Description = description,
                IsPublic = isPublic
            };

            _db.Lists.Add(playlist);
            _db.SaveChanges();
            return playlist;
        }

        public UserList? UpdatePlaylist(Guid currentUserId, Guid playlistId, string title, string? description, bool isPublic)
        {
            var playlist = _db.Lists
                .FirstOrDefault(l => l.Id == playlistId && l.UserId == currentUserId);

            if (playlist == null)
                return null;

            playlist.Title = title;
            playlist.Description = description;
            playlist.IsPublic = isPublic;

            _db.SaveChanges();
            return playlist;
        }


        public bool AddItemToPlaylist(Guid listId, string itemId, bool isMedia, Guid currentUserId)
        {
            var list = _db.Lists.Include(l => l.Media)
                                .Include(l => l.People)
                                .FirstOrDefault(l => l.Id == listId);

            if (list == null || list.UserId != currentUserId) return false;

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

        public List<UserList> GetPlaylistsByUserId(Guid userId, Guid? loggedUserId)
        {
            IQueryable<UserList> playlist = _db.Lists
                .Where(p => p.UserId == userId)
                .Include(p => p.User)
                .Include(p => p.People)
                .Include(p => p.Media)
                .ThenInclude(m => m.Titles);

            if (loggedUserId == null || loggedUserId != userId)
            {

                playlist = playlist.Where(p => p.IsPublic);

            }

            return playlist.ToList();
        }

        public UserList? GetPlaylistDetailByPlaylistId(Guid playlistId, Guid? loggedUserId)
        {
            return _db.Lists
                .Where(p => p.Id == playlistId &&
                    (p.IsPublic || p.UserId == loggedUserId))
                .Include(p => p.User)
                .Include(p => p.People)
                .Include(p => p.Media)
                .ThenInclude(m => m.Titles)
                .FirstOrDefault();
        }

        public List<Guid> IsMediaInPlaylists(string mediaId, Guid userId)
        {
            var userPlaylists = _db.Lists
                .Include(l => l.Media)
                .Where(l => l.UserId == userId && l.Media.Any(m => m.Id == mediaId))
                .Select(x => x.Id)
                .ToList();
            return userPlaylists;
        }

        public List<Guid> IsPeopleInPlaylists(string peopleId, Guid userId)
        {
            var userPlaylists = _db.Lists
                .Include(l => l.Media)
                .Where(l => l.UserId == userId && l.People.Any(m => m.Id == peopleId))
                .Select(x => x.Id)
                .ToList();
            return userPlaylists;
        }
    }
}
