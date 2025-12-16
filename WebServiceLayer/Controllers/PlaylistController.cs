using Microsoft.AspNetCore.Mvc;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using WebServiceLayer.DTOs.Requests;
using WebServiceLayer.DTOs.Responses;

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

        [HttpPost]
        [Authorize]
        public IActionResult CreatePlaylist([FromBody] CreatePlaylistDTO request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null) return Unauthorized();

            var currentUserId = Guid.Parse(userIdClaim.Value);
            var playlist = _playlistService.CreatePlaylist(currentUserId, request.Title, request.Description, request.IsPublic);
            return Ok(playlist);
        }

        [HttpPut("{playlistId}")]
        [Authorize]
        public IActionResult UpdatePlaylist([FromRoute] Guid playlistId, [FromBody] CreatePlaylistDTO request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null) return Unauthorized();

            var currentUserId = Guid.Parse(userIdClaim.Value);
            var playlist = _playlistService.UpdatePlaylist(currentUserId, playlistId, request.Title, request.Description, request.IsPublic);
            return Ok(playlist);
        }

        [HttpPost("{playlistId}/add")]
        [Authorize]
        public IActionResult AddItemToPlaylist(Guid playlistId, [FromBody] AddItemToPlaylistDTO request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null) return Unauthorized();

            var currentUserId = Guid.Parse(userIdClaim.Value);
            var result = _playlistService.AddItemToPlaylist(playlistId, request.ItemId, request.IsMedia, currentUserId);
            if (!result) return BadRequest(new { message = "Failed to add item" });
            return Ok(result);
        }

        [HttpDelete("{playlistId}/remove/{itemId}")]
        [Authorize]
        public IActionResult RemoveItemFromPlaylist([FromRoute] Guid playlistId, string itemId, [FromQuery] bool isMedia)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null) return Unauthorized();

            var currentUserId = Guid.Parse(userIdClaim.Value);
            var result = _playlistService.RemoveItemFromPlaylist(playlistId, itemId, isMedia, currentUserId);
            if (!result) return BadRequest(new { message = "Failed to remove item" });
            return Ok(result);
        }

        [HttpDelete("{playlistId}")]
        [Authorize]
        public IActionResult DeletePlaylist(Guid playlistId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null) return Unauthorized();

            var currentUserId = Guid.Parse(userIdClaim.Value);
            var result = _playlistService.DeletePlaylist(playlistId, currentUserId);
            if (!result) return NotFound(new { message = "Playlist not found or not owned by you" });
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetPlaylistsByUserId(Guid userId)
        {
            var playlists = _playlistService.GetPlaylistsByUserId(userId);
            var dto = playlists.Select(MapToDTO).ToList();
            return Ok(dto);
        }

        [HttpGet("{playlistId}")]
        public IActionResult GetPlaylistsByPlaylistId(Guid playlistId)
        {
            var playlist = _playlistService.GetPlaylistByPlaylistId(playlistId);

            if (playlist == null) return NotFound();
            var dto = MapToDTO(playlist);
            return Ok(dto);
        }

        private PlaylistDTO MapToDTO(UserList playlist)
        {
            var playlistDto = new PlaylistDTO
            {
                Id = playlist.Id,
                UserId = playlist.UserId!.Value,
                Title = playlist.Title,
                Description = playlist.Description,
                IsPublic = playlist.IsPublic,
                CreatedAt = playlist.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = playlist.UpdatedAt ?? DateTime.UtcNow,
                MediaIds = playlist.Media.Select(m => m.Id).ToList(),
                PeopleIds = playlist.People.Select(p => p.Id).ToList(),
                User = new UserDTO { Id = playlist.User.Id, Username = playlist.User.Username }
            };

            var mediaDtoList = playlist.Media.Select(m => new MediaDTO
            {
                Id = m.Id,
                DisplayTitle = m.DisplayTitle,
                ReleaseYear = m.ReleaseYear,
                AgeRating = m.AgeRating,
                Poster = m.Poster,
                AverageRating = m.AverageRating,
                ImdbAverageRating = m.ImdbAverageRating,
            }).ToList();

            playlistDto.Media = mediaDtoList;
            return playlistDto;
        }
    }
}
