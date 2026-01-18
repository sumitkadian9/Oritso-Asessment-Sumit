using TaskManagement.API.DTOs;
using TaskManagement.API.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace TaskManagement.API.Attributes;

public class TMAuthorize(params Role[] allowedRoles) : Attribute, IAuthorizationFilter
{
    private readonly Role[] _allowedRoles = allowedRoles;
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if(context != null)
        {
            StringValues authTokens;
            context.HttpContext.Request.Headers.TryGetValue("Authorization", out authTokens);
            string _token = authTokens.FirstOrDefault();
            if(_token != null)
            {
                UserDto user = (UserDto)context.HttpContext.Items["User"];
                if(user is null)
                {
                    SetUnauthorizedResult(context, "Invalid token");
                }

                if(_allowedRoles.Length != 0 && !_allowedRoles.Contains(user.Role))
                {
                    SetForbiddenResult(context, "Access denied");
                }
            }
            else
            {
                SetUnauthorizedResult(context, "Invalid token");
            }   
        }
    }

    private static void SetUnauthorizedResult(AuthorizationFilterContext context, string message)
    {
        var problemDetails = new ProblemDetails
        {
            Title = ErrorCode.NotAuthorized.ToString(),
            Detail = message,
            Status = StatusCodes.Status401Unauthorized
        };

        context.Result = new UnauthorizedObjectResult(problemDetails);
    }

    private static void SetForbiddenResult(AuthorizationFilterContext context, string message)
    {
        var problemDetails = new ProblemDetails
        {
            Title = ErrorCode.AccessDenied.ToString(),
            Detail = message,
            Status = StatusCodes.Status403Forbidden
        };

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = StatusCodes.Status403Forbidden
        };
    }
}