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
        private readonly IEpisodeService _episodeService;
        protected readonly IMapper _mapper;

        public MediaController(IMediaService mediaService, IReviewService reviewService, IPeopleService peopleService, IEpisodeService episodeService, LinkGenerator generator, IMapper mapper) : base(generator)
        {
            _mediaService = mediaService;
            _reviewService = reviewService;
            _peopleService = peopleService;
            _episodeService = episodeService;
            _mapper = mapper;
        }

        [HttpGet("{mediaId}", Name = nameof(GetMediaById))]
        public IActionResult GetMediaById([FromRoute] string mediaId)
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

        [HttpGet("{mediaId}/episodes", Name = nameof(GetMediaEpisodes))]
        public ActionResult<List<EpisodeList>> GetMediaEpisodes(string mediaId)
        {
            var mediaEpisodes = _episodeService.GetEpisodeList(mediaId);
            return Ok(mediaEpisodes);

        }

        private ReviewWithRating CreateRatingListModel(ReviewWithRating rating)
        {
            var model = _mapper.Map<ReviewWithRating>(rating);
            model.MediaUrl = GetUrl(nameof(GetMediaById), new { mediaId = rating.MediaId });
            model.UserUrl = GetUrl(nameof(UserController.UserDetails), new { id = rating.UserId });
            return model;
        }

        [HttpGet(Name = nameof(GetMediaList))]
        public async Task<IActionResult> GetMediaList([FromQuery] QueryParams queryParams, [FromQuery] string? orderBy = null)
        {
            var validSort = MediaSortBy.ReleaseYear;

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                var pascalCaseSortBy = ToPascalCase(orderBy);

                if (Enum.TryParse<MediaSortBy>(pascalCaseSortBy, ignoreCase: true, out var parsedSort))
                {
                    validSort = parsedSort;
                }
            }

            var (items, total) = await _mediaService.GetAllMedia(queryParams.Page, queryParams.PageSize, validSort);

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
                dvdReleaseDate = m.DvdRelease?.ReleaseDate,
                episode = m.EpisodeEpisodeMedia,
                averageRating = m.AverageRating,
                imdbRating = m.ImdbAverageRating,
                mediaType = m.MediaType
            });

            var result = CreatePaging(nameof(GetMediaList), mapped, total, queryParams);
            return Ok(result);
        }

        private string ToPascalCase(string snakeCase)
        {
            var parts = snakeCase.Split('_');
            return string.Concat(parts.Select(p => char.ToUpper(p[0]) + p.Substring(1)));
        }
    }
}