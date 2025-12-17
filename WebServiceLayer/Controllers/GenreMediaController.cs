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

        [HttpGet("{genreName}", Name = nameof(GetMediaByGenre))]
        public IActionResult GetMediaByGenre(string genreName, [FromQuery] QueryParams queryParams)
        {
            var mediaList = _genreMediaService.GetMediaByGenre(genreName, queryParams.Page, queryParams.PageSize);
            if (mediaList == null || !mediaList.Any())
                return NotFound($"No media found for genre: {genreName}");

            var result = mediaList.Select(m => new GetMediaByGenreDTO
            {
                Id = m.Id,
                MediaUrl = GetUrl(nameof(MediaController.GetMediaById), new { mediaId = m.Id }),
                Title = m.DisplayTitle,
                ReleaseYear = m.ReleaseYear,
                Poster = m.Poster,
                AverageRating = m.AverageRating,
                ImdbRating = m.ImdbAverageRating,
                HasEpisodes = m.EpisodeSeriesMedia.Any()

            });

            return Ok(CreatePaging(nameof(GetMediaByGenre), result, result.Count(), queryParams));
        }
    }
}
