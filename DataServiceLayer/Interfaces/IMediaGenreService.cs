using DataServiceLayer.Models;
using System.Collections.Generic;

public interface IMediaGenreService
{
    List<Media> GetMediaByGenre(Guid genreGuid, int pageNumber = 1, int pageSize = 10);
}
