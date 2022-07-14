using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using App.Users.Domain.Entities;
using App.Users.Domain.Setup.Configuration;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace App.Users.Domain.Infrastructure.Services
{
    internal class JwtAuthTokenService : IAuthTokenService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtAuthTokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public Task<string> GenerateAuthToken([NotNull] UserEntity userEntity)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var authClaims = new List<Claim>();
            authClaims.Add(new Claim("user_id", userEntity.Id.ToString()));
            authClaims.Add(new Claim("username", userEntity.Username));
            authClaims.Add(new Claim("email", userEntity.Email));
            
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudience,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            
            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}