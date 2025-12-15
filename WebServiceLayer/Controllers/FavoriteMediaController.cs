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
    public class FavoriteMediaController : BaseController
    {
        private readonly IFavoriteService _favoriteService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public FavoriteMediaController(IFavoriteService favoriteService, LinkGenerator linkGenerator, IMapper mapper) : base(linkGenerator)
        {
            _favoriteService = favoriteService;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet(Name = nameof(GetFavoriteMedia))]
        public ActionResult<PaginationResult<FavoriteMediaDTO>> GetFavoriteMedia(Guid userId, [FromQuery] QueryParams queryParams)
        {
            var favorites = _favoriteService.GetFavoriteMedia(userId, queryParams.Page, queryParams.PageSize);

            if (favorites.TotalCount == 0) return NoContent();

            var dtos = favorites.FavoriteMedia.Select(mp =>
           {
               var dto = _mapper.Map<FavoriteMediaDTO>(mp);

               dto.Title = mp.Media?.Titles.OrderBy(x => x.Ordering)?.FirstOrDefault()?.Title1;
               dto.ImdbRating = mp.Media?.ImdbAverageRating;
               dto.ReleaseYear = mp.Media?.ReleaseYear;

               return dto;
           }).ToList();

            return Ok(CreatePaging(nameof(GetFavoriteMedia), dtos, favorites.TotalCount, queryParams));
        }

        [HttpPost]
        public ActionResult Post(Guid userId, [FromBody] AddFavoriteMediaRequest request)
        {
            var success = _favoriteService.FavoriteMedia(userId, request.MediaId);
            if (!success) return BadRequest("Failed while adding to favorites");
            var location = GetUrl(nameof(GetFavoriteMedia), new { userId });
            return Created(location!, new { message = "added" });
        }

        [HttpDelete("{mediaId}")]
        public ActionResult Delete([FromRoute] Guid userId, string mediaId)
        {
            var success = _favoriteService.UnfavoriteMedia(userId, mediaId);
            if (!success) return StatusCode(500, "Failed while removing from favorites");
            return Ok(new { message = "deleted" });
        }

        private string? GetUrl(string endpointName, object values)
        {
            return _linkGenerator.GetUriByName(HttpContext, endpointName, values);
        }
    }
}
