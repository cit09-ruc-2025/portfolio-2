using Microsoft.AspNetCore.Mvc;
using DataServiceLayer.Interfaces;
using WebServiceLayer.Models;
using System.Linq;
using WebServiceLayer.Controllers;
using DataServiceLayer.Models;

namespace WebServicesLayer.Controllers
{
    [ApiController]
    [Route("api/genre")]
    public class GenreMediaController : BaseController
    {
        private readonly IMediaGenreService _genreMediaService;

        public GenreMediaController(IMediaGenreService genreMediaService, LinkGenerator linkGenerator) : base(linkGenerator)
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
                MediaUrl = GetUrl(nameof(MediaController.GetMediaById), new { mediaId = m.Id }),
                Title = m.Titles.FirstOrDefault()?.Title1,
                ReleaseYear = m.ReleaseYear,
                Poster = m.Poster,
                AverageRating = m.AverageRating,
            });

            return Ok(result);
        }
    }
}
