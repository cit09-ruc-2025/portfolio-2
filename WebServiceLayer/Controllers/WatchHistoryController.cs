using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebServiceLayer.DTOs.Requests;
using WebServiceLayer.DTOs.Responses;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers
{
    [Authorize(Policy = "SameUser")]
    [Route("api/user/{userId:guid}/watch-history")]
    [ApiController]
    public class WatchHistoryController : BaseController
    {
        private readonly IWatchHistoryService _mediaService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public WatchHistoryController(IWatchHistoryService mediaService, LinkGenerator linkGenerator, IMapper mapper) : base(linkGenerator)
        {
            _mediaService = mediaService;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet(Name = nameof(Get))]
        public ActionResult<PaginationResult<WatchHistoryDTO>> Get(Guid userId, [FromQuery] QueryParams queryParams)
        {
            var watched = _mediaService.GetWatchHistory(userId, queryParams.Page, queryParams.PageSize);

            if (watched.TotalCount == 0) return NoContent();

            var dtos = watched.WatchHistory.Select(wh =>
            {
                var dto = _mapper.Map<WatchHistoryDTO>(wh);

                dto.ImdbAverageRating = wh.Media.ImdbAverageRating;
                dto.Title = wh.Media?.Titles?.OrderBy(x => x.Ordering).FirstOrDefault()?.Title1 ?? "";
                dto.ReleaseYear = wh.Media?.ReleaseYear;

                return dto;
            }).ToList();
            return Ok(CreatePaging(nameof(Get), dtos, watched.TotalCount, queryParams));
        }

        [HttpPost]
        public ActionResult Post(Guid userId, [FromBody] AddToWatchHistoryRequest request)
        {
            var success = _mediaService.AddToWatched(request.MediaId, userId);
            if (!success) return BadRequest("Failed while adding to WatchHistory");
            var location = GetUrl(nameof(Get), new { userId });
            return Created(location!, new
            {
                message = "created"
            });
        }

        [HttpDelete("{mediaId}")]
        public ActionResult Delete(Guid userId, [FromRoute] string mediaId)
        {
            var success = _mediaService.RemoveFromWatched(mediaId, userId);
            if (!success) return BadRequest("Failed while removing from WatchHistory");
            return Ok(new { message = "removed" });
        }

        private string? GetUrl(string endpointName, object values)
        {
            return _linkGenerator.GetUriByName(HttpContext, endpointName, values);
        }
    }
}
