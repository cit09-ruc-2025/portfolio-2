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

        [HttpGet("{genreGuid}", Name = nameof(GetMediaByGenre))]
        public IActionResult GetMediaByGenre(Guid genreGuid, [FromQuery] QueryParams queryParams)
        {
            var mediaList = _genreMediaService.GetMediaByGenre(genreGuid, queryParams.Page, queryParams.PageSize);
            if (mediaList == null || !mediaList.Any())
                return NotFound($"No media found for genre: {genreGuid}");

            var result = mediaList.Select(m => new GetMediaByGenreDTO
            {
                Id = m.Id,
                MediaUrl = GetUrl(nameof(MediaController.GetMediaById), new { mediaId = m.Id }),
                Title = m.DisplayTitle,
                ReleaseYear = m.ReleaseYear,
                Poster = m.Poster,
                AverageRating = m.AverageRating,
                ImdbRating = m.ImdbAverageRating
            });

            return Ok(CreatePaging(nameof(GetMediaByGenre), result, result.Count(), queryParams));
        }
    }
}
