using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Dtos;
using DataServiceLayer.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : BaseController
    {
        private readonly IMediaService _mediaService;
        private readonly IPeopleService _peopleService;
        private readonly ISearchHistoryService _searchHistoryService;
        protected readonly IMapper _mapper;


        public SearchController(IMediaService mediaService, IPeopleService peopleService, ISearchHistoryService searchHistoryService, LinkGenerator generator, IMapper mapper) : base(generator)
        {
            _mediaService = mediaService;
            _peopleService = peopleService;
            _searchHistoryService = searchHistoryService;
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

        [HttpPost]
        public IActionResult AddToSearch([FromQuery] string keyword)
        {
            var id = User.FindFirst("id")?.Value;
            if (Guid.TryParse(id, out var userId))
            {
                _searchHistoryService.Add(keyword, userId);
            }
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public IActionResult DeleteSearchList()
        {
            var userId = Guid.Parse(User.FindFirst("id")!.Value);

            _searchHistoryService.Delete(userId);

            return Ok();
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetSearchList()
        {
            var userId = Guid.Parse(User.FindFirst("id")!.Value);

            var searchHistory = _searchHistoryService.List(userId);

            return Ok(searchHistory);
        }
    }
}