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
        private readonly IMediaGenreService _genreMediaService;

        public GenreMediaController(IMediaGenreService genreMediaService)
        {
            _genreMediaService = genreMediaService;
        }

        [HttpGet("{genreName}")]
        public IActionResult GetMediaByGenre(string genreName, int page = 1, int pageSize = 10)
        {
            var mediaList = _genreMediaService.GetMediaByGenre(genreName, page, pageSize);
            if (mediaList == null || !mediaList.Any())
                return NotFound($"No media found for genre: {genreName}");

            var result = mediaList.Select(m => new GetMediaByGenreDTO
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
