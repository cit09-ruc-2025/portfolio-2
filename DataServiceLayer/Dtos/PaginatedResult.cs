using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServiceLayer.Dtos
{

    public class PaginatedResult<T>
    {
        public IList<T> Items { get; set; }
        public int Total { get; set; }
    }

}