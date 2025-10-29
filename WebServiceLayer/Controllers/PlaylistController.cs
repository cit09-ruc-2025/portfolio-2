using Microsoft.AspNetCore.Mvc;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using System;
using Microsoft.Extensions.Configuration.UserSecrets;
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
        public IActionResult CreatePlaylist([FromBody] PlaylistCreateRequest request)
        {
            var playlist = _playlistService.CreatePlaylist(request.UserId, request.Title);
            return Ok(playlist);
        }

        [HttpPost("user/{userId}/{playlistId}/add")]
        public IActionResult AddMediaToPlaylist(
            [FromRoute] Guid userId,
            [FromRoute] Guid playlistId,
            [FromBody] AddMediaRequest request)
        {
            var playlist = _playlistService.GetPlaylistById(playlistId);
            if (playlist == null) return NotFound(new { message = "Playlist not found" });
            if (playlist.UserId != userId) return Forbid("You do not have permission");

            var result = _playlistService.AddMediaToPlaylist(playlistId, request.MediaId);
            return Ok(result);
        }

        [HttpPost("user/{userId}/{playlistId}/remove")]
        public IActionResult RemoveMediaFromPlaylist(
            [FromRoute] Guid userId,
            [FromRoute] Guid playlistId,
            [FromBody] AddMediaRequest request)
        {
            var playlist = _playlistService.GetPlaylistById(playlistId);
            if (playlist == null) return NotFound(new { message = "Playlist not found" });
            if (playlist.UserId != userId) return Forbid("You do not have permission");

            var result = _playlistService.RemoveMediaFromPlaylist(playlistId, request.MediaId);
            return Ok(result);
        }

        [HttpDelete("user/{userId}/{playlistId}")]
        public IActionResult DeletePlaylist(
            [FromRoute] Guid userId,
            [FromRoute] Guid playlistId)
        {
            var playlist = _playlistService.GetPlaylistById(playlistId);
            if (playlist == null) return NotFound(new { message = "Playlist not found" });
            if (playlist.UserId != userId) return Forbid("You do not have permission");

            var result = _playlistService.DeletePlaylist(playlistId);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetPlaylistsByUserId(Guid userId)
        {
            var playlists = _playlistService.GetPlaylistsByUserId(userId);
            return Ok(playlists);
        }
    }
}