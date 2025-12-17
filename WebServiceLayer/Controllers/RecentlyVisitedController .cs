using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Dtos;
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
    [Route("api/recently-visited")]
    public class RecentlyVisitedController : BaseController
    {
        private readonly IRecentlyVisited _recentlyVisitedService;
        private readonly IMediaService _mediaService;
        private readonly IPeopleService _peopleService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        public RecentlyVisitedController(IRecentlyVisited recentlyVisitedService, IMediaService mediaService, IPeopleService peopleService, LinkGenerator linkGenerator, IMapper mapper) : base(linkGenerator)
        {
            _recentlyVisitedService = recentlyVisitedService;
            _mediaService = mediaService;
            _peopleService = peopleService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddToRecent([FromBody] AddToRecent recentRecord)
        {
            var userId = Guid.Parse(User.FindFirst("id")!.Value);

            if (string.IsNullOrWhiteSpace(recentRecord?.MediaId) && string.IsNullOrWhiteSpace(recentRecord?.PeopleId))
            {
                return BadRequest(new { message = "PEOPLE_OR_MEDIA_REQUIRED" });

            }

            if (!string.IsNullOrWhiteSpace(recentRecord?.MediaId) && !string.IsNullOrWhiteSpace(recentRecord?.PeopleId))
            {
                return BadRequest(new { message = "ONLY_PEOPLE_OR_MEDIA" });
            }

            if (!string.IsNullOrWhiteSpace(recentRecord?.MediaId))
            {
                var media = _mediaService.GetById(recentRecord.MediaId);
                if (media == null)
                {
                    return NotFound(new { message = "Media does not exist" });
                }
            }

            if (!string.IsNullOrWhiteSpace(recentRecord?.PeopleId))
            {
                var people = _peopleService.GetPersonById(recentRecord.PeopleId);
                if (people == null)
                {
                    return NotFound(new { message = "PEOPLE_NOT_FOUND" });
                }
            }

            _recentlyVisitedService.Add(recentRecord?.MediaId, recentRecord?.PeopleId, userId);

            return Ok();
        }

        [Authorize]
        [HttpGet(Name = nameof(ListRecentlyVisited))]
        public IActionResult ListRecentlyVisited([FromQuery] QueryParams queryParams)
        {
            var userId = Guid.Parse(User.FindFirst("id")!.Value);

            var recentlyVisited = _recentlyVisitedService.List(userId, queryParams.Page, queryParams.PageSize);

            var mappedList = recentlyVisited.Item1.Select(x => CreateRecentlyViewed(x));

            return Ok(CreatePaging(nameof(ListRecentlyVisited), mappedList, recentlyVisited.count, queryParams));
        }

        private RecentlyVisitedDTO CreateRecentlyViewed(RecentlyViewed recentlyVisited)
        {
            var model = _mapper.Map<RecentlyVisitedDTO>(recentlyVisited);


            if (recentlyVisited.MediaId != null)
            {
                model.Media = _mapper.Map<MediaDTO>(recentlyVisited.Media!);
                model.Media.DisplayTitle = recentlyVisited
                    .Media!
                    .Titles
                    ?.OrderBy(x => x.Ordering)
                    ?.FirstOrDefault()
                    ?.Title1;
                model.Media.HasEpisodes = recentlyVisited.Media.EpisodeSeriesMedia.Any();

                model.MediaUrl = GetUrl(
                    nameof(MediaController.GetMediaById),
                    new { mediaId = recentlyVisited.MediaId }
                );

                model.PeopleUrl = null;
            }
            else if (recentlyVisited.PeopleId != null)
            {
                model.People = _mapper.Map<PeopleDTO>(recentlyVisited.People!);
                model.PeopleUrl = GetUrl(
                    nameof(PeopleController.GetPeople),
                    new { peopleId = recentlyVisited.PeopleId }
                );
                model.MediaUrl = null;

            }

            return model;
        }
    }
}