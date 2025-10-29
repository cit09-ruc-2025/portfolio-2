using Microsoft.AspNetCore.Mvc;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using System;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        [HttpPost("create")]
        public IActionResult CreatePlaylist([FromQuery] Guid userID, [FromQuery] string title)
        {
            var playlist = _playlistService.CreatePlaylist(userID, title);
            return Ok(playlist);
        }

        [HttpPost("addMedia")]
        public IActionResult AddMediaToPlaylist(Guid playlistId, string mediaId)
        {
            var result = _playlistService.AddMediaToPlaylist(playlistId, mediaId);
            return Ok(result);
        }

        [HttpPost("removeMedia")]
        public IActionResult RemoveMediaFromPlaylist(Guid playlistId, string mediaId)
        {
            var result = _playlistService.RemoveMediaFromPlaylist(playlistId, mediaId);
            return Ok(result);
        }

        [HttpDelete("delete/{playlistId}")]
        public IActionResult DeletePlaylist(Guid playlistId)
        {
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