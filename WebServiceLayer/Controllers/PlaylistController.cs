using Microsoft.AspNetCore.Mvc;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using WebServiceLayer.Models;
using MapsterMapper;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/playlist")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        [HttpPost("create")]
        public ActionResult<GetPlaylistByUserIdDTO> CreatePlaylist([FromBody] CreatePlaylistDTO request)
        {
            var playlist = _playlistService.CreatePlaylist(request.UserId, request.Title, request.Description);
            var dto = MapToDTO(playlist);
            return Ok(dto);
        }

        [HttpPost("{playlistId}/add")]
        public IActionResult AddItemToPlaylist(
            [FromRoute] Guid playlistId,
            [FromBody] AddItemToPlaylistDTO request)
        {
            var loggedInUserId = Guid.Parse(User.FindFirst("id")?.Value ?? Guid.Empty.ToString());
            var playlist = _playlistService.GetPlaylistsByUserId(loggedInUserId)
                                           .FirstOrDefault(p => p.Id == playlistId);

            if (playlist == null)
                return NotFound(new { message = "Playlist not found" });

            if (playlist.UserId != loggedInUserId)
                return Unauthorized(new { message = "You do not own this playlist" });

            var result = _playlistService.AddItemToPlaylist(playlistId, request.ItemId, request.IsMedia);
            return Ok(result);
        }

        [HttpPost("{playlistId}/remove")]
        public IActionResult RemoveItemFromPlaylist(
            [FromRoute] Guid playlistId,
            [FromBody] AddItemToPlaylistDTO request)
        {
            var loggedInUserId = Guid.Parse(User.FindFirst("id")?.Value ?? Guid.Empty.ToString());
            var playlist = _playlistService.GetPlaylistsByUserId(loggedInUserId)
                                           .FirstOrDefault(p => p.Id == playlistId);

            if (playlist == null)
                return NotFound(new { message = "Playlist not found" });

            if (playlist.UserId != loggedInUserId)
                return Unauthorized(new { message = "You do not own this playlist" });

            var result = _playlistService.RemoveItemFromPlaylist(playlistId, request.ItemId, request.IsMedia);
            return Ok(result);
        }

        [HttpDelete("{playlistId}")]
        public IActionResult DeletePlaylist([FromRoute] Guid playlistId)
        {
            var loggedInUserId = Guid.Parse(User.FindFirst("id")?.Value ?? Guid.Empty.ToString());
            var playlist = _playlistService.GetPlaylistsByUserId(loggedInUserId)
                                           .FirstOrDefault(p => p.Id == playlistId);

            if (playlist == null)
                return NotFound(new { message = "Playlist not found" });

            if (playlist.UserId != loggedInUserId)
                return Unauthorized(new { message = "You do not own this playlist" });

            var result = _playlistService.DeletePlaylist(playlistId);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetPlaylistsByUserId(Guid userId)
        {
            var playlists = _playlistService.GetPlaylistsByUserId(userId);

            var dto = playlists.Select(p => MapToDTO(p)).ToList();

            return Ok(dto);
        }

        private GetPlaylistByUserIdDTO MapToDTO(UserList playlist)
        {
            return new GetPlaylistByUserIdDTO
            {
                Id = playlist.Id,
                UserId = Guid.Parse(playlist.UserId?.ToString()),
                Title = playlist.Title,
                Description = playlist.Description,
                CreatedAt = playlist.CreatedAt ?? DateTime.MinValue,  // handle nullable
                UpdatedAt = playlist.UpdatedAt ?? DateTime.MinValue,  // handle nullable
                MediaListItems = playlist.Media.Select(mi => new MediaListItemDTO
                {
                    MediaId = mi.Id
                }).ToList(),
                PeopleListItems = playlist.People.Select(pi => new PeopleListItemDTO
                {
                    PeopleId = pi.Id
                }).ToList()
            };
        }

    }
}
