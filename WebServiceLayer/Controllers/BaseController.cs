using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers
{

    public class BaseController : ControllerBase
    {
        protected readonly LinkGenerator _generator;

        public BaseController(LinkGenerator generator)
        {
            _generator = generator;
        }

        protected PaginationResult<T> CreatePaging<T>(string endpointName, IEnumerable<T> items, int numberOfItems, QueryParams queryParams)
        {
            var numberOfPages = (int)Math.Ceiling((double)numberOfItems / queryParams.PageSize);

            var prev = queryParams.Page > 1
                ? GetUrl(endpointName, new { page = queryParams.Page - 1, queryParams.PageSize })
                : null;

            var next = queryParams.Page < numberOfPages
                ? GetUrl(endpointName, new { page = queryParams.Page + 1, queryParams.PageSize })
                : null;

            var first = GetUrl(endpointName, new { page = 1, queryParams.PageSize });
            var cur = GetUrl(endpointName, new { queryParams.Page, queryParams.PageSize });
            var last = GetUrl(endpointName, new { page = numberOfPages, queryParams.PageSize });

            return new PaginationResult<T>(
                first,
                prev,
                next,
                last,
                cur,
                numberOfPages,
                numberOfItems,
                items);
        }

        protected string? GetUrl(string endpointName, object values)
        {
            var routeValues = new RouteValueDictionary(HttpContext.GetRouteData().Values);

            Console.WriteLine(endpointName);

            Console.WriteLine(_generator.GetUriByName(HttpContext, endpointName, routeValues));

            return _generator.GetUriByName(HttpContext, endpointName, routeValues);

        }

        public record PaginationResult<T>(
            string? First,
            string? Prev,
            string? Next,
            string? Last,
            string? Current,
            int NumberOfPages,
            int NumberOfItems,
            IEnumerable<T> Items
        );
    }
}