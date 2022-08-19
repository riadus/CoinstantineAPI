using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CoinstantineAPI.Core;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using Microsoft.IdentityModel.Tokens;

namespace CoinstantineAPI.Users
{
    public class TokenService : ITokenService
    {
        private readonly IPasswordService _passwordService;

        public TokenService(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public RefreshTokens GetRefreshTokenObject(string refreshToken)
        {
            var (hash, salt) = _passwordService.CreatePasswordHash(refreshToken);
            var tokenDuration = Constants.RefreshTokenDurationInHours;
            return new RefreshTokens
            {
                RefreshToken = hash,
                RefreshTokenSalt = salt,
                GenerationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddHours(tokenDuration)
            };
        }

        public string GenerateTokenFor(UserIdentity user)
        {
            var key = Constants.Jwt; 
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var header = new JwtHeader(credentials);
            var tokenDuration = Constants.TokenDurationInHours;
            var payload = new JwtPayload
           {
                { ClaimTypes.NameIdentifier, user.UserId },
                { ClaimTypes.Email, user.EmailAddress },
                { ClaimTypes.Role, user.Role.ToString() },
                { "exp" , DateTimeOffset.UtcNow.AddHours(tokenDuration).ToUnixTimeSeconds()},
                { "iss" , Constants.AzureTenant},
                { "aud" , Constants.AzureClientId}
           };

            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(secToken);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Constants.Jwt)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidAudience = Constants.AzureClientId,
                ValidIssuer = Constants.AzureTenant
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
