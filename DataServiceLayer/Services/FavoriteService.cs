using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using DataServiceLayer.Helpers;

namespace DataServiceLayer.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly string? _connectionString;

        public FavoriteService(string? connectionString)
        {
            _connectionString = connectionString;
        }
        private MediaDbContext CreateContext() => new(_connectionString);


        public bool FavoriteMedia(Guid userId, string mediaId)
        {
            try
            {
                var context = CreateContext();

                var favoriteExists = context.FavoriteMedia.Any(p => p.UserId == userId && p.MediaId == mediaId);
                if (favoriteExists) return true;

                context.Add(new FavoriteMedia() { UserId = userId, CreatedAt = DateTime.Now, MediaId = mediaId });
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool FavoritePerson(Guid userId, string peopleId)
        {
            try
            {
                var context = CreateContext();

                var favoriteExists = context.FavoritePeople.Any(p => p.UserId == userId && p.PeopleId == peopleId);
                if (favoriteExists) return true;

                context.Add(new FavoritePerson() { UserId = userId, CreatedAt = DateTime.Now, PeopleId = peopleId });
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UnfavoriteMedia(Guid userId, string mediaId)
        {
            try
            {
                var context = CreateContext();

                var foundMedia = context.FavoriteMedia.FirstOrDefault(fm => fm.UserId == userId && fm.MediaId == mediaId);

                if (foundMedia != null)
                {
                    context.Remove(foundMedia);
                    context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UnfavoritePerson(Guid userId, string peopleId)
        {
            try
            {
                var context = CreateContext();

                var foundPerson = context.FavoritePeople.FirstOrDefault(fm => fm.UserId == userId && fm.PeopleId == peopleId);

                if (foundPerson != null)
                {
                    context.Remove(foundPerson);
                    context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public (List<FavoritePerson> FavoritePeople, int TotalCount) GetFavoritePeople(Guid userId, int pageNumber, int pageSize)
        {
            var context = CreateContext();
            return context.FavoritePeople.Where(fp => fp.UserId == userId).GetPaginatedResult(pageNumber, pageSize);
        }

        public (List<FavoriteMedia> FavoriteMedia, int TotalCount) GetFavoriteMedia(Guid userId, int pageNumber, int pageSize)
        {
            var context = CreateContext();
            return context.FavoriteMedia.Where(fp => fp.UserId == userId).GetPaginatedResult(pageNumber, pageSize);
        }

        public bool IsMediaFavorite(string mediaId, Guid userId)
        {
            var db = new MediaDbContext(_connectionString);

            return db.FavoriteMedia.Any(x => x.MediaId == mediaId && x.UserId == userId);
        }
    }
}
