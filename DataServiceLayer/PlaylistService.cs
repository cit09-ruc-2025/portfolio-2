using DataServiceLayer.Models;
using DataServiceLayer.Interfaces;

namespace DataServiceLayer.Services
{
    public class PlaylistService : IPlaylistService
    {
        private List<Playlist> playlists = new List<Playlist>();

        public Playlist CreatePlaylist(Guid userID, string title)
        {
            var playlist = new Playlist
            {
                Id = Guid.NewGuid(),
                UserId = userID,
                Title = title,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            playlists.Add(playlist);

            return playlist;
        }

        public bool AddMediaToPlaylist(Guid playlistId, string mediaId)
        {
            var playlist = playlists.FirstOrDefault(p => p.Id == playlistId);
            if (playlist != null)
            {
                var playlistItem = new PlaylistItem
                {
                    MediaId = mediaId,
                    PlaylistId = playlistId
                };
                playlist.PlaylistItems.Add(playlistItem);
                return true;
            }
            return false;
        }

        public bool RemoveMediaFromPlaylist(Guid playlistId, string mediaId)
        {
            var playlist = playlists.FirstOrDefault(p => p.Id == playlistId);
            if (playlist != null)
            {
                var item = playlist.PlaylistItems.FirstOrDefault(i => i.MediaId == mediaId);
                if (item != null)
                {
                    playlist.PlaylistItems.Remove(item);
                    playlist.UpdatedAt = DateTime.UtcNow;
                    return true;
                }
            }
            return false;
        }

        public bool DeletePlaylist(Guid playlistId)
        {
            var playlist = playlists.FirstOrDefault(p => p.Id == playlistId);
            if (playlist != null)
            {
                playlists.Remove(playlist);
                return true;
            }
            return false;
        }

        public List<Playlist> GetPlaylistsByUserId(Guid userId)
        {
            return playlists.Where(p => p.UserId == userId).ToList();
        }

    }
}