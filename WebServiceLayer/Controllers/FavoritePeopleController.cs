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
    [Route("api/user/{userId:guid}/favorite-people")]
    [ApiController]
    public class FavoritePeopleController : BaseController
    {
        private readonly IFavoriteService _favoriteService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public FavoritePeopleController(IFavoriteService favoriteService, LinkGenerator linkGenerator, IMapper mapper) : base(linkGenerator)
        {
            _favoriteService = favoriteService;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet(Name = nameof(GetFavoritePeople))]
        public ActionResult<PaginationResult<FavoritePeopleDTO>> GetFavoritePeople(Guid userId, [FromQuery] QueryParams queryParams)
        {
            var favorites = _favoriteService.GetFavoritePeople(userId, queryParams.Page, queryParams.PageSize);

            if (favorites.TotalCount == 0) return NoContent();

            var dtos = favorites.FavoritePeople.Select(mp =>
          {
              var dto = _mapper.Map<FavoritePeopleDTO>(mp);

              dto.Name = mp.People.Name;

              return dto;
          }).ToList();


            return Ok(CreatePaging(nameof(GetFavoritePeople), dtos, favorites.TotalCount, queryParams));
        }

        [HttpPost]
        public ActionResult Post(Guid userId, [FromBody] AddFavoritePeopleRequest request)
        {
            var success = _favoriteService.FavoritePerson(userId, request.PeopleId);
            if (!success) return BadRequest("Failed while adding to favorites");
            var location = GetUrl(nameof(GetFavoritePeople), new { userId });
            return Created(location!, new { message = "added" });
        }

        [HttpDelete("{peopleId}")]
        public ActionResult Delete([FromRoute] Guid userId, string peopleId)
        {
            var success = _favoriteService.UnfavoritePerson(userId, peopleId);
            if (!success) return StatusCode(500, "Failed while removing from favorites");
            return Ok(new { message = "deleted" });
        }

        private string? GetUrl(string endpointName, object values)
        {
            return _linkGenerator.GetUriByName(HttpContext, endpointName, values);
        }
    }
}
