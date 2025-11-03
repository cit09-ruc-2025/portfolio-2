using DataServiceLayer.Models;

namespace DataServiceLayer.Interfaces;

public interface IGenreService
{
    (List<Genre> Genres, int TotalCount) GetAllGenres(int pageNumber, int pageSize);
}