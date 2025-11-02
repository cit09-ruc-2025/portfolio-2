using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Dtos;
using DataServiceLayer.DTOs;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/media")]
    public class MediaController : BaseController
    {
        private readonly IMediaService _mediaService;
        private readonly IReviewService _reviewService;
        protected readonly IMapper _mapper;

        public MediaController(IMediaService mediaService, IReviewService reviewService, LinkGenerator generator, IMapper mapper) : base(generator)
        {
            _mediaService = mediaService;
            _reviewService = reviewService;
            _mapper = mapper;
        }

        [HttpGet("{mediaId}/reviews", Name = nameof(ReviewList))]
        public IActionResult ReviewList([FromRoute] string mediaId, [FromQuery] QueryParams queryParams)
        {
            var media = _mediaService.GetById(mediaId);

            if (media == null)
            {
                return NotFound(new
                {
                    errors = new
                    {
                        media = "MEDIA_NOT_FOUND"
                    }
                });
            }

            var ratings = _reviewService
                .GetByMediaId(mediaId, queryParams.Page, queryParams.PageSize);

            var mappedRatings = ratings.Items
            .Select(x => CreateRatingListModel(x));

            if (ratings.Items.Count < 1)
            {
                return NoContent();
            }

            var results = CreatePaging(nameof(ReviewList), mappedRatings, ratings.Total, queryParams);

            return Ok(results);
        }

        private ReviewWithRating CreateRatingListModel(ReviewWithRating rating)
        {
            var model = _mapper.Map<ReviewWithRating>(rating);
            model.UserUrl = GetUrl(nameof(ReviewList), new { id = rating.UserId });
            return model;
        }

    }
}