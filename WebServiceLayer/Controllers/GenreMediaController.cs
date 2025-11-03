using Microsoft.AspNetCore.Mvc;
using DataServiceLayer.Interfaces;
using WebServiceLayer.Models;
using System.Linq;

namespace WebServicesLayer.Controllers
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
        public IActionResult GetMediaByGenre(string genreName)
        {
            var mediaList = _genreMediaService.GetMediaByGenre(genreName);

            if (mediaList == null || !mediaList.Any())
                return NotFound($"No media found for genre: {genreName}");

            // Map to DTO
            var result = mediaList.Select(m => new GetMediaByGenreRequest
            {
                MediaId = m.Id,
                Title = m.Titles.FirstOrDefault()?.Title1,
                ReleaseYear = m.ReleaseYear,
                Poster = m.Poster,
                AverageRating = m.AverageRating,
            });

            return Ok(result);
        }
    }
}
