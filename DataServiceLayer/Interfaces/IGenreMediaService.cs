using DataServiceLayer.Models;
using System.Collections.Generic;

public interface IGenreMediaService
{
    List<Media> GetMediaByGenre(string genreName, int pageNumber = 0, int pageSize = 10);
}
