using DataServiceLayer.Dtos;
using DataServiceLayer.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.DTOs.Responses;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : BaseController
    {
        private readonly IPeopleService _peopleService;
        private readonly IMediaService _mediaService;
        private readonly IFavoriteService _favoriteService;
        private readonly IMapper _mapper;

        public PeopleController(IPeopleService peopleService, IMediaService mediaService, IFavoriteService favoriteService, IMapper mapper, LinkGenerator generator) : base(generator)
        {
            _peopleService = peopleService;
            _mediaService = mediaService;
            _favoriteService = favoriteService;

            _mapper = mapper;
        }

        [HttpGet("{peopleId}", Name = nameof(GetPeople))]
        public ActionResult<PeopleDTO> GetPeople(string peopleId)
        {
            var person = _peopleService.GetPersonById(peopleId);

            if (person == null) return NotFound();

            var dto = _mapper.Map<PeopleDTO>(person);
            return Ok(dto);
        }

        [HttpGet("{peopleId}/media", Name = nameof(GetPeopleMedia))]
        public ActionResult<PeopleDTO> GetPeopleMedia([FromRoute] string peopleId, [FromQuery] QueryParams queryParams)
        {
            var peopleMedia = _mediaService.GetMediaForPeople(peopleId, queryParams.Page, queryParams.PageSize);

            if (peopleMedia.TotalCount == 0) return NoContent();

            var dtos = peopleMedia.PeopleMedia.Select(mp =>
            {
                var dto = _mapper.Map<PeopleMediaDTO>(mp);

                dto.MediaId = mp.MediaId;
                dto.Title = mp.Media?.Titles.OrderBy(x => x.Ordering)?.FirstOrDefault()?.Title1;
                dto.ReleaseYear = mp.Media?.ReleaseYear ?? 0;
                dto.ImdbAverageRating = mp.Media?.ImdbAverageRating;

                return dto;
            }).ToList();

            return Ok(CreatePaging(nameof(GetPeopleMedia), dtos, peopleMedia.TotalCount, queryParams));
        }

        [Authorize]
        [HttpGet("{peopleId}/user-status")]
        public ActionResult<PeopleUserStatus> GetMediaUserStatus([FromRoute] string peopleId)
        {
            var userId = Guid.Parse(User.FindFirst("id")!.Value);

            var isPeopleFavorite = _favoriteService.IsMediaFavorite(peopleId, userId);

            var peopleUserStatus = new PeopleUserStatus
            {
                IsFavorite = isPeopleFavorite,

            };

            return Ok(peopleUserStatus);

        }
    }
}
