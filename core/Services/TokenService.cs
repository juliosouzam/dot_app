using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using core.Interfaces;
using core.Models;
using Microsoft.IdentityModel.Tokens;

namespace core.Services
{
    public class TokenService(IConfiguration configuration) : ITokenService
    {
        private readonly IConfiguration _configuration = configuration;

        private readonly SymmetricSecurityKey _key = new(Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]));

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.GivenName, user.UserName)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}