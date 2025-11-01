using DataServiceLayer.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.DTOs.Responses;

namespace WebServiceLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleService _peopleService;
        private readonly IMapper _mapper;

        public PeopleController(IPeopleService peopleService, IMapper mapper)
        {
            _peopleService = peopleService;
            _mapper = mapper;
        }

        [HttpGet("peopleId")]
        public ActionResult<PeopleDTO> Get(string peopleId)
        {
            var person = _peopleService.GetPersonById(peopleId);

            if (person == null) return NotFound();

            var dto = _mapper.Map<PeopleDTO>(person);
            var media = person.MediaPeople.Select(mp => mp.Media).DistinctBy(m => m.Id).ToList();
            dto.KnownFor = _mapper.Map<List<MediaDTO>>(media);

            return Ok(dto);
        }
    }
}
