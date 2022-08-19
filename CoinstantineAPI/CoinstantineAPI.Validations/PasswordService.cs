using System;
using System.Security.Cryptography;
using System.Text;
using CoinstantineAPI.Core.Users;

namespace CoinstantineAPI.Users
{
    public class PasswordService : IPasswordService
    {
        public (string Hash, string Salt) CreatePasswordHash(string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

            using (var hmac = new HMACSHA512())
            {
                return (Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password))), Convert.ToBase64String(hmac.Key));
            }
        }

        public bool VerifyPasswordHash(string password, string hash, string salt)
        {
            var hashBytes = Convert.FromBase64String(hash);
            var saltBytes = Convert.FromBase64String(salt);
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            if (hashBytes.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(hashBytes));
            if (saltBytes.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(saltBytes));

            using (var hmac = new HMACSHA512(saltBytes))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != hashBytes[i]) return false;
                }
            }

            return true;
        }
    }
}
