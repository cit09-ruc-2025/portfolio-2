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
            var existingUser = _db.Users.FirstOrDefault(u => u.Id == userID);
            if (existingUser == null)
            {
                throw new Exception("User does not exist.");
            }

            var playlist = new List
            {
                Id = Guid.NewGuid(),
                UserId = userID,  // must exist in Users table
                Title = title,
                Description = description,
                CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
                UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
            };

            _db.Lists.Add(playlist);
            _db.SaveChanges();
            return playlist;
        }

        public bool AddItemToPlaylist(Guid listId, string itemId, bool isMedia)
        {
            var list = _db.Lists.FirstOrDefault(l => l.Id == listId);
            if (list == null) return false;

            list.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

            if (isMedia)
            {
                if (_db.MediaListItems.Any(mi => mi.ListId == listId && mi.MediaId == itemId))
                    return false;

                var media = _db.Media.FirstOrDefault(m => m.Id == itemId);
                if (media == null) return false;

                _db.MediaListItems.Add(new MediaListItem
                {
                    ListId = listId,
                    MediaId = itemId
                });
            }
            else
            {
                if (_db.PeopleListItems.Any(pi => pi.ListId == listId && pi.PeopleId == itemId))
                    return false;

                var person = _db.People.FirstOrDefault(p => p.Id == itemId);
                if (person == null) return false;

                _db.PeopleListItems.Add(new PeopleListItem
                {
                    ListId = listId,
                    PeopleId = itemId
                });
            }

            _db.SaveChanges();
            return true;
        }
        public bool RemoveItemFromPlaylist(Guid listId, string itemId, bool isMedia)
        {
            if (isMedia)
            {
                var mediaItem = _db.MediaListItems.FirstOrDefault(mi => mi.ListId == listId && mi.MediaId == itemId);
                if (mediaItem == null) return false;

                _db.MediaListItems.Remove(mediaItem);
            }
            else
            {
                var personItem = _db.PeopleListItems.FirstOrDefault(pi => pi.ListId == listId && pi.PeopleId == itemId);
                if (personItem == null) return false;

                _db.PeopleListItems.Remove(personItem);
            }

            var list = _db.Lists.FirstOrDefault(l => l.Id == listId);
            if (list != null)
                list.UpdatedAt = DateTime.UtcNow;

            _db.SaveChanges();
            return true;
        }

        public bool DeletePlaylist(Guid listId)
        {
            var list = _db.Lists.FirstOrDefault(l => l.Id == listId);
            if (list == null) return false;

            var mediaItems = _db.MediaListItems.Where(mi => mi.ListId == listId).ToList();
            var peopleItems = _db.PeopleListItems.Where(pi => pi.ListId == listId).ToList();

            _db.MediaListItems.RemoveRange(mediaItems);
            _db.PeopleListItems.RemoveRange(peopleItems);
            _db.Lists.Remove(list);

            _db.SaveChanges();
            return true;
        }

        public List<List> GetPlaylistsByUserId(Guid userId)
        {
            return _db.Lists
            .Where(p => p.UserId == userId)
            .ToList();
        }

        public List? GetPlaylistById(Guid listId)
        {
            return _db.Lists
            .FirstOrDefault(p => p.Id == listId);
        }

    }
}