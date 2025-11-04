using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/actor")]
    public class ActorController : ControllerBase
    {
        private readonly IActorService _actorService;

        public ActorController(IActorService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet("{actorName}/coactors")]
        public IActionResult GetCoActors(string actorName, int page = 1, int pageSize = 10)
        {
            var coActors = _actorService.GetCoActors(actorName);

            if (coActors.Count == 0)
                return NotFound($"No co-actors found for actor '{actorName}'.");

            return Ok(coActors);
        }
    }
}
