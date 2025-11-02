using Microsoft.AspNetCore.Mvc;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using System;
using Microsoft.Extensions.Configuration.UserSecrets;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/genre")]
    public class GenreMediaController : ControllerBase
    {
        private readonly IGenreMediaService _genreMediaService;

        public GenreMediaController(IGenreMediaService genreMediaService)
        {
            _genreMediaService = genreMediaService;
        }

        [HttpGet("{genreName}")]
        public IActionResult GetMediaByGenre([FromRoute] string genreName)
        {
            var media = _genreMediaService.GetMediaByGenre(genreName);

            if (!media.Any())
                return NotFound($"No media found for genre: {genreName}");

            return Ok(media);
        }
    }
}