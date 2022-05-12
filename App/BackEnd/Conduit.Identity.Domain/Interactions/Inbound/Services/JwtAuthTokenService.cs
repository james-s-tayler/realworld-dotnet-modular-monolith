using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Conduit.Identity.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Conduit.Identity.Domain.Interactions.Inbound.Services
{
    public class JwtAuthTokenService : IAuthTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtAuthTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<string> GenerateAuthToken(User user)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var authClaims = new List<Claim>();
            authClaims.Add(new Claim("user_id", user.Id.ToString()));
            authClaims.Add(new Claim("username", user.Username));
            authClaims.Add(new Claim("email", user.Email));
            
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            
            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}