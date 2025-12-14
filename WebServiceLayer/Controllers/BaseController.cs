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
                ? GetUrl(endpointName, new { page = queryParams.Page - 1, pageSize = queryParams.PageSize }, includeAllRouteValues: true)
                : null;

            var next = queryParams.Page < numberOfPages
                ? GetUrl(endpointName, new { page = queryParams.Page + 1, pageSize = queryParams.PageSize }, includeAllRouteValues: true)
                : null;

            var first = GetUrl(endpointName, new { page = 1, pageSize = queryParams.PageSize }, includeAllRouteValues: true);
            var cur = GetUrl(endpointName, new { page = queryParams.Page, pageSize = queryParams.PageSize }, includeAllRouteValues: true);
            var last = GetUrl(endpointName, new { page = numberOfPages, pageSize = queryParams.PageSize }, includeAllRouteValues: true);

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

        protected string? GetUrl(string endpointName, object values, bool includeAllRouteValues = false)
        {
            RouteValueDictionary routeValues;

            if (includeAllRouteValues)
            {
                routeValues = new RouteValueDictionary(HttpContext.GetRouteData().Values);
                foreach (var kv in new RouteValueDictionary(values))
                {
                    routeValues[kv.Key] = kv.Value;
                }
            }
            else
            {
                routeValues = new RouteValueDictionary(values);
            }

            return _generator.GetUriByName(
                HttpContext,
                endpointName,
                routeValues);
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