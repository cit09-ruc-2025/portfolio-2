using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Models;

namespace DataServiceLayer.Interfaces
{
    public interface IUserService
    {
        User? GetUserByEmail(string email);
        User? GetUserByUsername(string username);
        void CreateUser(User user);
    }
}