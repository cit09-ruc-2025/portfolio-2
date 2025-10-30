using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/rating")]
    public class RatingController : ControllerBase
    {

        private readonly IRatingService _ratingService;
        private readonly IMediaService _mediaService;

        public RatingController(IRatingService ratingService, IMediaService mediaService)
        {
            _ratingService = ratingService;
            _mediaService = mediaService;
        }

        [Authorize]
        [HttpPut("{mediaId}")]
        public async Task<IActionResult> AddReview([FromRoute] string mediaId, [FromBody] AddRatingRequest rating)
        {
            if (string.IsNullOrWhiteSpace(mediaId))
            {
                return BadRequest(new
                {
                    errors = new
                    {
                        mediaId = "MEDIA_ID_REQUIRED"
                    }
                });
            }

            var media = _mediaService.GetById(mediaId);

            if (media == null)
            {
                return NotFound(new
                {
                    errors = new
                    {
                        mediaId = "MEDIA_NOT_FOUND"
                    }
                });
            }

            var userId = Guid.Parse(User.FindFirst("id")?.Value);


            var newRating = new Rating
            {
                MediaId = mediaId,
                UserId = userId,
                Rating1 = rating.Rating
            };

            await _ratingService.UpsertRating(newRating);

            return Created();
        }

    }
}