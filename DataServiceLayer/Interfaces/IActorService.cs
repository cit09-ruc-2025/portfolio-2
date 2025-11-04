using DataServiceLayer.Models;
using System.Collections.Generic;

namespace DataServiceLayer.Interfaces
{
    public interface IActorService
    {
        List<CoActor> GetCoActors(string actorName, int pageNumber = 1, int pageSize = 10);
    }
}
