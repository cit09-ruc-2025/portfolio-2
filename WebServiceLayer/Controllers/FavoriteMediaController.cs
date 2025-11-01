using DataServiceLayer.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.DTOs.Requests;
using WebServiceLayer.DTOs.Responses;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers
{
    [Authorize(Policy = "SameUser")]
    [Route("api/user/{userId:guid}/favorite-media")]
    [ApiController]
    public class FavoriteMediaController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public FavoriteMediaController(IFavoriteService favoriteService, LinkGenerator linkGenerator, IMapper mapper)
        {
            _favoriteService = favoriteService;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet(Name = nameof(GetFavoriteMedia))]
        public ActionResult GetFavoriteMedia(Guid userId)
        {
            var favorites = _favoriteService.GetFavoriteMedia(userId);

            if (favorites == null || favorites.Count == 0) return NoContent();

            return Ok(_mapper.Map<List<FavoriteMediaDTO>>(favorites));
        }

        [HttpPost]
        public ActionResult Post(Guid userId, [FromBody] AddFavoriteMediaRequest request)
        {
            var success = _favoriteService.FavoriteMedia(userId, request.MediaId);
            if (!success) return BadRequest("Failed while adding to favorites");
            var location = GetUrl(nameof(GetFavoriteMedia), new { userId });
            return Created(location!, null);
        }

        [HttpDelete("peopleId")]
        public ActionResult Delete(Guid userId, string mediaId)
        {
            var success = _favoriteService.UnfavoriteMedia(userId, mediaId);
            if (!success) return StatusCode(500, "Failed while removing from favorites");
            return NoContent();
        }

        private string? GetUrl(string endpointName, object values)
        {
            return _linkGenerator.GetUriByName(HttpContext, endpointName, values);
        }
    }
}
