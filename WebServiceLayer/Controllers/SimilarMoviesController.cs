using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebServiceLayer.Models;

namespace WebServicesLayer.Controllers
{
    [ApiController]
    [Route("api/similar")]
    public class SimilarMovieController : ControllerBase
    {
        private readonly ISimilarMovieService _similarMovieService;

        public SimilarMovieController(ISimilarMovieService similarMovieService)
        {
            _similarMovieService = similarMovieService;
        }

        [HttpGet("{mediaId}")]
        public ActionResult<List<GetSimilarMoviesDTO>> GetSimilarMovies(string mediaId, [FromQuery] int limit = 5)
        {
            var movies = _similarMovieService.GetSimilarMovies(mediaId, limit);

            var response = movies.Select(m => new GetSimilarMoviesDTO
            {
                MediaId = m.Id,
                Title = m.DisplayTitle,
                ReleaseYear = m.ReleaseYear,
                Poster = m.Poster,
                AverageRating = m.AverageRating,
                Plot = m.Plot
            }).ToList();

            if (response.Count == 0)
                return NotFound(new { message = "No similar movies found." });

            return Ok(response);
        }
    }
}
