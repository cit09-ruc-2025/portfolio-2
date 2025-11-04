using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataServiceLayer.Helpers;
using DataServiceLayer.Dtos;

namespace DataServiceLayer.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly string? _connectionString;

        public PeopleService(string? connectionString)
        {
            _connectionString = connectionString;
        }

        public (List<MediaPerson> MediaPeople, int TotalCount) GetPeopleForMedia(string mediaId, int pageNumber, int pageSize)
        {
            var context = CreateContext();

            var baseQuery = context.MediaPeople
                .Include(p => p.People)
                .Include(mp => mp.Role)
                .Where(mp => mp.MediaId == mediaId)
                .OrderBy(mp => mp.Ordering);

            return baseQuery.GetPaginatedResult(pageNumber, pageSize);
        }

        public Person? GetPersonById(string peopleId)
        {
            var context = CreateContext();
            return context.People.Include(p => p.MediaPeople.Where(mp => mp.KnownFor == true)).ThenInclude(mp => mp.Media).ThenInclude(m => m.Titles.Where(t => t.Ordering == 1)).FirstOrDefault(p => p.Id == peopleId);
        }

        public PaginatedResult<PeopleList> GetByName(string keyword, int page, int pageSize)
        {
            var db = new MediaDbContext(_connectionString);

            var query = db.People
                .Where(x => x.Name.ToLower().Contains(keyword.ToLower()));

            var result = new PaginatedResult<PeopleList>
            {
                Total = query.Count(),
                Items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PeopleList
                {
                    PeopleId = p.Id,
                    Name = p.Name
                })
                .ToList()
            };
            return result;
        }

        private MediaDbContext CreateContext() => new(_connectionString);


    }
}
