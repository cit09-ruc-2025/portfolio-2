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

        protected object CreatePaging<T>(string endpointName, IEnumerable<T> items, int numberOfItems, QueryParams queryParams)
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

            return new
            {
                First = first,
                Prev = prev,
                Next = next,
                Last = last,
                Current = cur,
                NumberOfPages = numberOfPages,
                NumberOfIems = numberOfItems,
                Items = items
            };
        }

        protected string? GetUrl(string endpointName, object values)
        {
            var routeValues = new RouteValueDictionary(HttpContext.GetRouteData().Values);

            foreach (var kv in new RouteValueDictionary(values))
                routeValues[kv.Key] = kv.Value;

            return _generator.GetUriByName(HttpContext, endpointName, routeValues);

        }

    }
}