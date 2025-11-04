using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Dtos;
using DataServiceLayer.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : BaseController
    {
        private readonly IMediaService _mediaService;
        private readonly IPeopleService _peopleService;
        protected readonly IMapper _mapper;


        public SearchController(IMediaService mediaService, IPeopleService peopleService, LinkGenerator generator, IMapper mapper) : base(generator)
        {
            _mediaService = mediaService;
            _peopleService = peopleService;
            _mapper = mapper;
        }

        [HttpGet("media", Name = nameof(SearchMedia))]
        public IActionResult SearchMedia([FromQuery] QueryParams queryParams, [FromQuery] string keyword)
        {
            var movieList = _mediaService.GetByTitle(keyword, queryParams.Page, queryParams.PageSize);

            var paginatedMovies = CreatePaging(nameof(SearchMedia), movieList.Items, movieList.Total, queryParams);

            return Ok(paginatedMovies);
        }

        [HttpGet("people", Name = nameof(SearchPeople))]
        public IActionResult SearchPeople([FromQuery] QueryParams queryParams, [FromQuery] string keyword)
        {
            var peopleByName = _peopleService.GetByName(keyword, queryParams.Page, queryParams.PageSize);

            var paginatedMovies = CreatePaging(nameof(SearchPeople), peopleByName.Items, peopleByName.Total, queryParams);

            return Ok(peopleByName);
        }
    }
}