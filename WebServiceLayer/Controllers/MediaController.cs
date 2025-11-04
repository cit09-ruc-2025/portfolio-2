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
using WebServiceLayer.DTOs.Responses;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/media")]
    public class MediaController : BaseController
    {
        private readonly IMediaService _mediaService;
        private readonly IReviewService _reviewService;
        private readonly IPeopleService _peopleService;
        protected readonly IMapper _mapper;

        public MediaController(IMediaService mediaService, IReviewService reviewService, IPeopleService peopleService, LinkGenerator generator, IMapper mapper) : base(generator)
        {
            _mediaService = mediaService;
            _reviewService = reviewService;
            _peopleService = peopleService;
            _mapper = mapper;
        }

        [HttpGet("{mediaId}", Name = nameof(GetMediaById))]
        public IActionResult GetMediaById([FromRoute] string mediaId)
        {
            var media = _mediaService.GetById(mediaId);

            return Ok(media);
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

        [HttpGet("{mediaId}/people", Name = nameof(GetPeopleForMedia))]
        public ActionResult<PaginationResult<MediaPeopleDTO>> GetPeopleForMedia(string mediaId, [FromQuery] QueryParams queryParams)
        {
            var mediaPeopleWithCount = _peopleService.GetPeopleForMedia(mediaId, queryParams.Page, queryParams.PageSize);

            if (mediaPeopleWithCount.TotalCount == 0) return NoContent();

            var dtos = mediaPeopleWithCount.MediaPeople.Select(mp =>
            {
                var dto = _mapper.Map<MediaPeopleDTO>(mp);

                dto.Description = !string.IsNullOrWhiteSpace(mp.JobNote) ? mp.JobNote : mp.Characters;
                dto.PersonName = mp.People?.Name;
                dto.RoleName = mp.Role?.Name;

                return dto;
            }).ToList();

            return Ok(CreatePaging(nameof(GetPeopleForMedia), dtos, mediaPeopleWithCount.TotalCount, queryParams));
        }

        private ReviewWithRating CreateRatingListModel(ReviewWithRating rating)
        {
            var model = _mapper.Map<ReviewWithRating>(rating);
            model.UserUrl = GetUrl(nameof(ReviewList), new { id = rating.UserId });
            return model;
        }

        [HttpGet(Name = nameof(GetMediaList))]
        public async Task<IActionResult> GetMediaList([FromQuery] QueryParams queryParams)
        {
            var (items, total) = await _mediaService.GetAllMedia(queryParams.Page, queryParams.PageSize);

            if (items.Count == 0)
                return NoContent();

            var mapped = items.Select(m => new
            {
                id = m.Id,
                title = m.DisplayTitle,
                releaseYear = m.ReleaseYear,
                ageRating = m.AgeRating,
                poster = m.Poster,
                genres = m.Genres.Select(g => g.Name),
                dvdReleaseDate = m.DvdRelease?.ReleaseDate
            });

            var result = CreatePaging(nameof(GetMediaList), mapped, total, queryParams);
            return Ok(result);
        }

    }
}