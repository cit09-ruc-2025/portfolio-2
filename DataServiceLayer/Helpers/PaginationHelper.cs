using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Helpers
{
    public static class PaginationHelper
    {
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public static (List<T> Items, int TotalCount) GetPaginatedResult<T>(this IQueryable<T> baseQuery, int pageNumber, int pageSize)
        {
            var totalCount = baseQuery.Count();

            var items = baseQuery.ApplyPagination(pageNumber, pageSize).ToList();
            return (items, totalCount);
        }
    }
}
