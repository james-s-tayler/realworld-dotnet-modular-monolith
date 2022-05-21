using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Conduit.Identity.Domain.Configuration;
using Conduit.Identity.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Conduit.Identity.Domain.Infrastructure.Services
{
    public class JwtAuthTokenService : IAuthTokenService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtAuthTokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public Task<string> GenerateAuthToken(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var authClaims = new List<Claim>();
            authClaims.Add(new Claim("user_id", user.Id.ToString()));
            authClaims.Add(new Claim("username", user.Username));
            authClaims.Add(new Claim("email", user.Email));
            
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