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

        public class PlaylistCreateRequest
        {
            public Guid UserId { get; set; }
            public string Title { get; set; } = null!;
        }

        [HttpPost("create")]
        public IActionResult CreatePlaylist([FromBody] PlaylistCreateRequest request)
        {
            var playlist = _playlistService.CreatePlaylist(request.UserId, request.Title);
            return Ok(playlist);
        }

        public class AddMediaRequest
        {
            public Guid PlaylistId { get; set; }
            public string MediaId { get; set; } = null!;
        }

        [HttpPost("addMedia")]
        public IActionResult AddMediaToPlaylist([FromBody] AddMediaRequest request)
        {
            var result = _playlistService.AddMediaToPlaylist(request.PlaylistId, request.MediaId);
            return Ok(result);
        }

        public class RemoveMediaRequest
        {
            public Guid PlaylistId { get; set; }
            public string MediaId { get; set; } = null!;
        }

        [HttpPost("removeMedia")]
        public IActionResult RemoveMediaFromPlaylist([FromBody] RemoveMediaRequest request)
        {
            var result = _playlistService.RemoveMediaFromPlaylist(request.PlaylistId, request.MediaId);
            return Ok(result);
        }

        public class DeletePlaylistRequest
        {
            public Guid PlaylistId { get; set; }
        }

        [HttpDelete("delete")]
        public IActionResult DeletePlaylist([FromBody] DeletePlaylistRequest request)
        {
            var result = _playlistService.DeletePlaylist(request.PlaylistId);
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