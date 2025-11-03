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

        public IEnumerable<Genre> GetAllGenres(int pageNumber, int pageSize)
        {
            var db = new MediaDbContext(_connectionString);
            return db.Genres.ApplyPagination(pageNumber, pageSize).ToList();
        }

    }
}
