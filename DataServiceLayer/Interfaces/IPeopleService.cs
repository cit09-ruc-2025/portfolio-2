using DataServiceLayer.Dtos;
using DataServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Interfaces
{
        public interface IPeopleService
        {
                Person? GetPersonById(string peopleId);
                (List<MediaPerson> MediaPeople, int TotalCount) GetPeopleForMedia(string mediaId, int pageNumber, int pageSize);
                public PaginatedResult<PeopleList> GetByName(string keyword, int page, int pageSize);
        }
}
