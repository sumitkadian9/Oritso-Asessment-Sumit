using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TaskManagement.API.DTOs;
using TaskManagement.API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace TaskManagement.API.Middlewares;

public class AuthMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private readonly RequestDelegate _next = next;
    private readonly IConfiguration _configuration = configuration;

    public async Task Invoke(HttpContext context, IUserService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            await AttachAccountToContext(context, token, userService);

        await _next(context);
    }

    private async Task AttachAccountToContext(HttpContext context, string token, IUserService userService)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["TokenKey"]);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            string userIdString = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value;

            // attach account to context on successful jwt validation
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                var res = await userService.GetUserById(userId);
                if(res.IsSuccess)
                {
                    UserDto user = res.Value;
                    // Attach user to context on successful jwt validation
                    context.Items["User"] = user;
                }
            }
        }
        catch
        {
            // do nothing if jwt validation fails
            // account is not attached to context so request won't have access to secure routes
        }
    }
}