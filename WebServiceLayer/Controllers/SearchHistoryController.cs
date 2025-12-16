using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers
{
    [Authorize]
    [Route("api/search-history")]
    public class SearchHistoryController : BaseController
    {
        private readonly ISearchHistoryService _searchHistoryService;
        private readonly LinkGenerator _linkGenerator;


        public SearchHistoryController(ISearchHistoryService searchHistoryService, LinkGenerator linkGenerator) : base(linkGenerator)
        {
            _searchHistoryService = searchHistoryService;
            _linkGenerator = linkGenerator;
        }

        [HttpPost]
        public IActionResult AddToSearch([FromQuery] string keyword)
        {
            var id = User.FindFirst("id")?.Value;
            if (Guid.TryParse(id, out var userId))
            {
                _searchHistoryService.Add(keyword, userId);
            }
            return Ok(
                new
                {
                    message = "Added"
                });
        }

        [HttpDelete]
        public IActionResult DeleteSearchList()
        {
            var userId = Guid.Parse(User.FindFirst("id")!.Value);

            _searchHistoryService.Delete(userId);

            return Ok(new
            {
                message = "Deleted"
            });
        }

        [HttpGet(Name = nameof(GetSearchList))]
        public IActionResult GetSearchList([FromQuery] QueryParams queryParams)
        {
            var userId = Guid.Parse(User.FindFirst("id")!.Value);

            var searchHistory = _searchHistoryService.List(userId, queryParams.Page, queryParams.PageSize);

            return Ok(CreatePaging(nameof(GetSearchList), searchHistory.Item1, searchHistory.count, queryParams));

        }
    }
}