using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;

namespace DataServiceLayer
{
    public class UserService : IUserService
    {
        private readonly string? _connectionString;

        public UserService(string? connectionString)
        {
            _connectionString = connectionString;
        }

        public User? GetUserByEmail(string email)
        {
            var db = new MediaDbContext(_connectionString);
            var user = db.Users.FirstOrDefault(x => x.Email == email);
            return user;
        }

        public User? GetUserByUsername(string username)
        {
            var db = new MediaDbContext(_connectionString);
            var user = db.Users.FirstOrDefault(x => x.Username == username);
            return user;
        }

        public void CreateUser(User user)
        {
            var db = new MediaDbContext(_connectionString);
            db.Users.Add(user);
            db.SaveChanges();
        }

    }
}
