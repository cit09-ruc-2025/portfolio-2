using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using DataServiceLayer.Helpers;

namespace DataServiceLayer.Services
{
    public class GenreService : IGenreService
    {
        private readonly string? _connectionString;

        public GenreService(string? connectionString)
        {
            _connectionString = connectionString;
        }

        public (List<Genre> Genres, int TotalCount) GetAllGenres(int pageNumber, int pageSize)
        {
            var db = new MediaDbContext(_connectionString);
            return db.Genres.GetPaginatedResult(pageNumber, pageSize);
        }

    }
}
