using Application.DTOS.Auth;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class TokenServices : ITokenServices
    {
        private readonly IConfiguration _configuration;

        public TokenServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetToken(string userId, string userName, string email, IList<string> roles)
        {
            var jwt = _configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

            if (jwt == null)
            {
                throw new InvalidOperationException("JwtOptions section is missing from appsettings.json");
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Name, userName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

         
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

          
            var token = new JwtSecurityToken(
                issuer: jwt.issure,
                audience: jwt.audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwt.Duration),
                signingCredentials: credentials
            );

          
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

