using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : BaseController
    {
        private readonly IMediaService _mediaService;

        public SearchController(IMediaService mediaService, LinkGenerator generator) : base(generator)
        {
            _mediaService = mediaService;
        }

        [HttpGet(Name = nameof(Search))]
        public IActionResult Search([FromQuery] QueryParams queryParams, [FromQuery] string keyword)
        {
            Console.WriteLine(keyword);
            var movieByTitle = _mediaService.GetByTitle(keyword, queryParams.Page, queryParams.PageSize);
            return Ok(movieByTitle);
        }
    }
}