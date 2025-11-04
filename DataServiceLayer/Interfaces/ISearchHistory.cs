using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Models;

namespace DataServiceLayer.Interfaces
{
    public interface ISearchHistoryService
    {
        public void Add(string keyword, Guid userId);
        public void Delete(Guid userId);
        public IList<SearchHistory> List(Guid userId);
    }
}