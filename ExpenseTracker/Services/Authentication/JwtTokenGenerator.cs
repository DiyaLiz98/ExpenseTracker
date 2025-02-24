using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExpenseTracker.Services.Authentication
{
    public static class JwtTokenGenerator
    {
        public static string GenerateToken(string userId, string role, string secretKey, string issuer, string audience, int expireMinutes)
        {
            //Header Info
            var algorithm = SecurityAlgorithms.HmacSha256;

            //payLoad Info
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //signature: header + payload + secretKey
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, algorithm);

            //Generate Token

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires : DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
