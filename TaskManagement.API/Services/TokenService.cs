using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace TaskManagement.API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    private readonly SymmetricSecurityKey _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

    public string CreateToken(Guid userId)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, userId.ToString())
        };

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        DateTime tokenExpires = DateTime.UtcNow.AddDays(1);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = tokenExpires,
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}