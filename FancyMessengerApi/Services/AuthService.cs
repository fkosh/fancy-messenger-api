using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FancyMessengerApi.Services
{
    public class AuthService
    {
        private static readonly JwtSecurityTokenHandler _handler;

        private readonly byte[] _secretKey;

        static AuthService()
        {
            _handler = new JwtSecurityTokenHandler();
        }

        public AuthService(string secretKey)
        {
            _secretKey = Encoding.UTF8.GetBytes(secretKey);
        }

        public string CreateUserToken(string userId)
        {
            var tokenId = Guid.NewGuid().ToString();

            var token = _handler.WriteToken(
                new JwtSecurityToken(
                    claims: new[]
                    {
                        new Claim("sub", userId),
                        new Claim("jti", tokenId)
                    },
                    expires: DateTime.UtcNow.AddDays(7),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(_secretKey),
                        SecurityAlgorithms.HmacSha256Signature
                    )
                )
            );

            // TODO _logger.Info($"Был создан токен {tokenId} для пользователя {userId}");

            return token;
        }

        // TODO out
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(
                    Encoding.UTF8.GetBytes(password)
                );
            }
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(
                    "Value cannot be empty or whitespace only string.", nameof(password)
                );

            if (storedHash.Length != 64)
                throw new ArgumentException(
                    "Invalid length of password hash (64 bytes expected).", "passwordHash"
                );

            if (storedSalt.Length != 128)
                throw new ArgumentException(
                    "Invalid length of password salt (128 bytes expected).", "passwordSalt"
                );

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}