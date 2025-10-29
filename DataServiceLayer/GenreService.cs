using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;

namespace DataServiceLayer
{
    public class GenreService : IGenreService
    {
        private readonly string? _connectionString;

        public GenreService(string? connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Genre> GetAllGenres()
        {
            var db = new MediaDbContext(_connectionString);
            return db.Genres.ToList();
        }

    }
}
