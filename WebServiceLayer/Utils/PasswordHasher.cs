using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using WebServiceLayer.Interfaces;


namespace WebServiceLayer.Utils
{
    public class PasswordHasher : IPasswordHasher
    {

        public string HashPassword(string password)
        {

            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            ));

            return hashedPassword;
        }

    }
}