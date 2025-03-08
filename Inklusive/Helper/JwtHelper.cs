using Inklusive.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Inklusive.Helper
{
    public class JwtHelper
    {
        private readonly IConfiguration _config;

        public JwtHelper(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1), // Token expires in 1 hour
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public List<Claim> ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero, // No additional tolerance for token expiration
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                // If the token is valid, return the claims
                return principal.Claims.ToList();
            }
            catch (Exception)
            {
                // If the token is invalid, return null
                return null;
            }
        }

    }
}
