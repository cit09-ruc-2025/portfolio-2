using DataServiceLayer.Models;
using DataServiceLayer.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BackendTests
{
    public class DataServiceLayerTests : IDisposable
    {
        private readonly string _connectionString;
        private readonly Guid _userId = Guid.Parse("79e60db1-ff30-404f-aa57-1c99bb82cdfc");


        public DataServiceLayerTests()
        {

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // Get connectionstring from WebServiceLayer appsettings.Development.json
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");

            var db = new MediaDbContext(_connectionString);
            var user = db.Users.Find(_userId);
            if (user == null)
            {
                var testUser = new User()
                {
                    Id = _userId,
                    Email = "test.user@email.mail",
                    HashedPassword = "1234".GetHashCode().ToString(),
                    Username = "TestUser"
                };
                db.Users.Add(testUser);
                db.SaveChanges();
            }
        }

        public void Dispose()
        {
            var db = new MediaDbContext(_connectionString);
            var user = db.Users.Find(_userId);

            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }
        }


        private GenreService GetGenreService() => new GenreService(_connectionString);
        private FavoriteService GetFavoriteService() => new FavoriteService(_connectionString);
        private PeopleService GetPeopleService() => new PeopleService(_connectionString);

        [Fact]
        public void GetAllGenres_ShouldReturnGenres()
        {
            // Arrange
            var genreService = GetGenreService();
            
            // Act
            var genres = genreService.GetAllGenres(1, 100).Genres;
            
            // Assert
            Assert.Equal(27, genres.Count);
            Assert.NotNull(genres);
            Assert.NotEmpty(genres);
        }

        [Fact]
        public void GetAllGenres_Page1_PageSize10_ShouldReturn10Genres()
        {
            // Arrange
            var genreService = GetGenreService();
            
            // Act
            var genres = genreService.GetAllGenres(1, 10).Genres;
            
            // Assert
            Assert.Equal(10, genres.Count);
            Assert.NotNull(genres);
            Assert.NotEmpty(genres);
        }

        [Fact]
        public void FavoriteMedia_ShouldAddFavoriteSuccessfully()
        {
            // Arrange
            var favoriteService = GetFavoriteService();

            var mediaId = "tt0407887";
            var userId = _userId;

            // Act
            var result = favoriteService.FavoriteMedia(userId, mediaId);
            
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UnfavoriteMedia_ShouldRemoveFavoriteSuccessfully()
        {
            // Arrange
            var favoriteService = GetFavoriteService();
            var mediaId = "tt0407887";
            var userId = _userId;

            // Act
            var result = favoriteService.UnfavoriteMedia(userId, mediaId);

            // Assert
            Assert.True(result);

        }

        [Fact]
        public void GetPeopleForMedia_ShouldReturnPeople()
        {
            // Arrange
            var peopleService = GetPeopleService();
            var mediaId = "tt0407887";
            
            // Act
            var (mediaPeople, totalCount) = peopleService.GetPeopleForMedia(mediaId, 1, 10);
            
            // Assert
            Assert.NotNull(mediaPeople);
            Assert.NotEmpty(mediaPeople);
            Assert.True(totalCount > 0);
        }

        [Theory]
        [InlineData("tt0407887", "Leonardo Dicaprio")]
        [InlineData("tt0098286", "Jerry Seinfeld")]
        [InlineData("tt0098936", "Kyle MacLachlan")]
        public void GetPeopleForMedia_ShouldIncludeNestedData(string mediaId, string nameOfFirstPerson)
        {
            // Arrange
            var peopleService = GetPeopleService();

            // Act
            var (mediaPeople, totalCount) = peopleService.GetPeopleForMedia(mediaId, 1, 10);

            // Assert
            var person = mediaPeople.FirstOrDefault()?.People;
            Assert.NotNull(person);
            Assert.Equal(nameOfFirstPerson, person.Name, ignoreCase: true);
            Assert.NotEmpty(mediaPeople);
            Assert.True(totalCount > 0);
        }
    }
}
