using DataServiceLayer.Models;

namespace DataServiceLayer.Interfaces;

public interface IGenreService
{
    IEnumerable<Genre> GetAllGenres(int pageNumber, int pageSize);
}