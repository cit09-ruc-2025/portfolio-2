using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebServiceLayer.DTOs.Requests;
using WebServiceLayer.DTOs.Responses;

namespace WebServiceLayer.Controllers
{
    [Authorize(Policy = "SameUser")]
    [Route("api/user/{userId:guid}/watch-history")]
    [ApiController]
    public class WatchHistoryController : ControllerBase
    {
        private readonly IWatchHistoryService _mediaService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public WatchHistoryController(IWatchHistoryService mediaService, LinkGenerator linkGenerator, IMapper mapper)
        {
            _mediaService = mediaService;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet(Name = nameof(Get))]
        public ActionResult<List<WatchHistoryDTO>> Get(Guid userId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var watched = _mediaService.GetWatchHistory(userId, pageNumber, pageSize);

            if (watched == null || watched.Count == 0) return NoContent();

            return Ok(_mapper.Map<List<WatchHistoryDTO>>(watched));
        }

        [HttpPost]
        public ActionResult Post(Guid userId, [FromBody] AddToWatchHistoryRequest request)
        {
            var success = _mediaService.AddToWatched(request.MediaId, userId);
            if (!success) return BadRequest("Failed while adding to WatchHistory");
            var location = GetUrl(nameof(Get), new { userId });
            return Created(location!, null);
        }

        [HttpDelete("mediaId")]
        public ActionResult Delete(Guid userId, string mediaId)
        {
            var success = _mediaService.RemoveFromWatched(mediaId, userId);
            if (!success) return BadRequest("Failed while removing from WatchHistory");
            return NoContent();
        }

        private string? GetUrl(string endpointName, object values)
        {
            return _linkGenerator.GetUriByName(HttpContext, endpointName, values);
        }
    }
}
