using DataServiceLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/genre")]
public class GenreController : ControllerBase
{
    private readonly IGenreService _genreService;
    
    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public IActionResult GetGenres([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var genres = _genreService.GetAllGenres(pageNumber, pageSize);
        return Ok(genres);
    }
    
}