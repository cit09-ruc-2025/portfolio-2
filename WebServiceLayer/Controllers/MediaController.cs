using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Dtos;
using DataServiceLayer.DTOs;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IFavoriteService _favoriteService;
        private readonly IWatchHistoryService _watchHistoryService;
        private readonly IPlaylistService _playlistService;
        protected readonly IMapper _mapper;

        public MediaController(IMediaService mediaService, IReviewService reviewService, IPeopleService peopleService, IEpisodeService episodeService, IFavoriteService favoriteService, IWatchHistoryService watchHistoryService, IPlaylistService playlistService, LinkGenerator generator, IMapper mapper) : base(generator)
        {
            _mediaService = mediaService;
            _reviewService = reviewService;
            _peopleService = peopleService;
            _episodeService = episodeService;
            _favoriteService = favoriteService;
            _watchHistoryService = watchHistoryService;
            _playlistService = playlistService;
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
                        media = "Media does not exist"
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
                        media = "Media does not exist"
                    }
                });
            }

            Guid? userId = null;

            var claim = User.FindFirst("id")?.Value;
            if (!string.IsNullOrEmpty(claim))
            {
                userId = Guid.Parse(claim);
            }


            var ratings = _reviewService
                .GetByMediaId(mediaId, queryParams.Page, queryParams.PageSize, userId);

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

        [Authorize]
        [HttpGet("{mediaId}/user-status")]
        public ActionResult<MediaUserStatus> GetMediaUserStatus(string mediaId)
        {
            var userId = Guid.Parse(User.FindFirst("id")!.Value);

            var userHasMediaReview = _reviewService.HasUserReviewed(mediaId, userId);

            var isMediaFavorite = _favoriteService.IsMediaFavorite(mediaId, userId);

            var isMediaWatched = _watchHistoryService.IsWatched(mediaId, userId);

            var playlists = _playlistService.IsMediaInPlaylists(mediaId, userId);

            var mediaUserStatus = new MediaUserStatus
            {
                IsFavorite = isMediaFavorite,
                IsReviewed = userHasMediaReview,
                IsWatched = isMediaWatched,
                Playlists = playlists
            };

            return Ok(mediaUserStatus);

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

            var (items, total) = _mediaService.GetAllMedia(queryParams.Page, queryParams.PageSize, validSort);

            if (items.Count == 0)
                return NoContent();


            var result = CreatePaging(nameof(GetMediaList), items, total, queryParams);
            return Ok(result);
        }

        private string ToPascalCase(string snakeCase)
        {
            var parts = snakeCase.Split('_');
            return string.Concat(parts.Select(p => char.ToUpper(p[0]) + p.Substring(1)));
        }
    }
}