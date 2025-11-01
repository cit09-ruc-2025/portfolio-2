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
    [Route("api/review")]
    public class ReviewController : ControllerBase
    {

        private readonly IReviewService _reviewService;
        private readonly IMediaService _mediaService;

        public ReviewController(IReviewService reviewService, IMediaService mediaService)
        {
            _reviewService = reviewService;
            _mediaService = mediaService;
        }

        [Authorize]
        [HttpPut("{mediaId}")]
        public async Task<IActionResult> AddReview([FromRoute] string mediaId, [FromBody] AddReviewRequest review)
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

            var newReview = new ReviewParam
            {
                MediaId = mediaId,
                UserId = userId,
                Rating = review.Rating,
                Review = review.Review
            };

            await _reviewService.UpsertReview(newReview);

            return Ok();
        }

    }
}