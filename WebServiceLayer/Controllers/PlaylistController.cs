using Microsoft.AspNetCore.Mvc;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using System;
using WebServiceLayer.Models;

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
        public IActionResult CreatePlaylist([FromBody] CreatePlaylistDTO request)
        {
            var playlist = _playlistService.CreatePlaylist(request.UserId, request.Title, request.Description);
            return Ok(playlist);
        }

        [HttpPost("user/{userId}/{playlistId}/add")]
        public IActionResult AddItemToPlaylist(
            [FromRoute] Guid userId,
            [FromRoute] Guid playlistId,
            [FromBody] AddItemToPlaylistDTO request)
        {
            var playlist = _playlistService.GetPlaylistById(playlistId);
            if (playlist == null)
                return NotFound(new { message = "Playlist not found" });

            if (playlist.UserId != userId)
                return Unauthorized(new { message = "You do not have permission" });

            var result = _playlistService.AddItemToPlaylist(playlistId, request.ItemId, request.IsMedia);
            return Ok(result);
        }

        [HttpPost("user/{userId}/{playlistId}/remove")]
        public IActionResult RemoveItemFromPlaylist(
            [FromRoute] Guid userId,
            [FromRoute] Guid playlistId,
            [FromBody] AddItemToPlaylistDTO request)
        {
            var playlist = _playlistService.GetPlaylistById(playlistId);
            if (playlist == null)
                return NotFound(new { message = "Playlist not found" });

            if (playlist.UserId != userId)
                return Unauthorized(new { message = "You do not have permission" });

            var result = _playlistService.RemoveItemFromPlaylist(playlistId, request.ItemId, request.IsMedia);
            return Ok(result);
        }

        [HttpDelete("user/{userId}/{playlistId}")]
        public IActionResult DeletePlaylist(
            [FromRoute] Guid userId,
            [FromRoute] Guid playlistId)
        {
            var playlist = _playlistService.GetPlaylistById(playlistId);
            if (playlist == null)
                return NotFound(new { message = "Playlist not found" });

            if (playlist.UserId != userId)
                return Unauthorized(new { message = "You do not have permission" });

            var result = _playlistService.DeletePlaylist(playlistId);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetPlaylistsByUserId([FromRoute] Guid userId)
        {
            var playlists = _playlistService.GetPlaylistsByUserId(userId);
            return Ok(playlists);
        }

        [HttpGet("{playlistId}")]
        public IActionResult GetPlaylistById([FromRoute] Guid playlistId)
        {
            var playlist = _playlistService.GetPlaylistById(playlistId);
            if (playlist == null)
                return NotFound(new { message = "Playlist not found" });

            return Ok(playlist);
        }
    }
}
