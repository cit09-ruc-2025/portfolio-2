using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Dtos;
using DataServiceLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/recently-visited")]
    public class RecentlyVisitedController : ControllerBase
    {
        private readonly IRecentlyVisited _recentlyVisitedService;
        private readonly IMediaService _mediaService;
        private readonly IPeopleService _peopleService;

        public RecentlyVisitedController(IRecentlyVisited recentlyVisitedService, IMediaService mediaService, IPeopleService peopleService)
        {
            _recentlyVisitedService = recentlyVisitedService;
            _mediaService = mediaService;
            _peopleService = peopleService;
        }

        [HttpPost]
        public IActionResult AddToRecent([FromBody] AddToRecent recentRecord)
        {
            var id = User.FindFirst("id")?.Value;
            if (Guid.TryParse(id, out var userId))
            {
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
                        return NotFound(new { message = "MEDIA_NOT_FOUND" });
                    }
                }

                if (!string.IsNullOrWhiteSpace(recentRecord?.PeopleId))
                {
                    var people = _peopleService.GetPersonById(recentRecord.PeopleId);
                    Console.WriteLine(people);
                    if (people == null)
                    {
                        return NotFound(new { message = "PEOPLE_NOT_FOUND" });
                    }
                }

                _recentlyVisitedService.Add(recentRecord?.MediaId, recentRecord?.PeopleId, userId);
            }
            return Ok();
        }
    }
}