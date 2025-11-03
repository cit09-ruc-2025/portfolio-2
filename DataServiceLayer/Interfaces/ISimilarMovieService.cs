using DataServiceLayer.Models;
using System.Collections.Generic;

namespace DataServiceLayer.Interfaces
{
    public interface ISimilarMovieService
    {
        List<Media> GetSimilarMovies(string movieId, int limit = 5);
    }
}
