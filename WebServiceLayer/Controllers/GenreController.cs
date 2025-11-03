using DataServiceLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models;
using WebServiceLayer.DTOs.Responses;
using MapsterMapper;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/genre")]
public class GenreController : BaseController
{
    private readonly IGenreService _genreService;
    private readonly IMapper _mapper;

    public GenreController(IGenreService genreService, IMapper mapper, LinkGenerator generator) : base(generator)
    {
        _genreService = genreService;
        _mapper = mapper;
    }

    [HttpGet(Name = nameof(GetGenres))]
    public ActionResult<PaginationResult<GenreDTO>> GetGenres([FromQuery] QueryParams queryParams)
    {
        var genres = _genreService.GetAllGenres(queryParams.Page, queryParams.PageSize);

        if (genres.TotalCount == 0) return NoContent();

        var dto = _mapper.Map<List<GenreDTO>>(genres.Genres);

        return Ok(CreatePaging(nameof(GetGenres), dto, genres.TotalCount, queryParams));
    }

}