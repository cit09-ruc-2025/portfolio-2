using DataServiceLayer.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.DTOs.Requests;
using WebServiceLayer.DTOs.Responses;

namespace WebServiceLayer.Controllers
{
    [Authorize(Policy = "SameUser")]
    [Route("api/user/{userId:guid}/favorite-people")]
    [ApiController]
    public class FavoritePeopleController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public FavoritePeopleController(IFavoriteService favoriteService, LinkGenerator linkGenerator, IMapper mapper)
        {
            _favoriteService = favoriteService;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet(Name = nameof(GetFavoritePeople))]
        public ActionResult GetFavoritePeople(Guid userId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var favorites = _favoriteService.GetFavoritePeople(userId, pageNumber, pageSize);

            if (favorites == null || favorites.Count == 0) return NoContent();

            return Ok(_mapper.Map<List<FavoritePeopleDTO>>(favorites));
        }

        [HttpPost]
        public ActionResult Post(Guid userId, [FromBody] AddFavoritePeopleRequest request)
        {
            var success = _favoriteService.FavoritePerson(userId, request.PeopleId);
            if (!success) return BadRequest("Failed while adding to favorites");
            var location = GetUrl(nameof(GetFavoritePeople), new { userId });
            return Created(location!, null);
        }

        [HttpDelete("peopleId")]
        public ActionResult Delete(Guid userId, string peopleId)
        {
            var success = _favoriteService.UnfavoritePerson(userId, peopleId);
            if (!success) return StatusCode(500, "Failed while removing from favorites");
            return NoContent();
        }

        private string? GetUrl(string endpointName, object values)
        {
            return _linkGenerator.GetUriByName(HttpContext, endpointName, values);
        }
    }
}
