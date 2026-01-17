using TaskManagement.API.Enums;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.API.Extensions;

public static class FluentResultExtensions
{
    // public static ActionResult ToProblemDetailsOrOk<T>(this Result<T> result, ControllerBase controller)
    // {
    //     if (result.IsSuccess)
    //     {
    //         return controller.Ok(result.Value);
    //     }

    //     var error = result.Errors.First();
    //     var errorCode = error.Metadata.GetValueOrDefault("ErrorCode") as ErrorCode?;

    //     var problemDetails = new ProblemDetails
    //     {
    //         Title = errorCode?.ToString() ?? "Error",
    //         Detail = error.Message,
    //         Status = errorCode switch
    //         {
    //             // Account related errors
    //             ErrorCode.DuplicateUser => StatusCodes.Status409Conflict,
    //             ErrorCode.RegistrationFailed => StatusCodes.Status400BadRequest,
    //             ErrorCode.UserNotFound => StatusCodes.Status404NotFound,
    //             ErrorCode.UserNotDeletable => StatusCodes.Status400BadRequest,
    //             ErrorCode.InvalidCredentials => StatusCodes.Status401Unauthorized,
    //             _ => StatusCodes.Status400BadRequest
    //         },
    //         // Type = $"https://example.com/errors/{errorCode?.ToString().ToLower() ?? "general-error"}"
    //     };

    //     return controller.StatusCode(problemDetails.Status.Value, problemDetails);
    // }

    // New in .NET 9
    public static ActionResult ToProblemDetailsOrOk<T>(this Result<T> result, ControllerBase controller)
    {
        if (result.IsSuccess) return controller.Ok(result.Value);

        var error = result.Errors.First();
        var errorCode = error.Metadata.GetValueOrDefault("ErrorCode") as ErrorCode?;

        int statusCode = errorCode switch
        {
            ErrorCode.UserNotFound => StatusCodes.Status404NotFound,
            ErrorCode.DuplicateUser => StatusCodes.Status409Conflict,
            ErrorCode.RegistrationFailed => StatusCodes.Status400BadRequest,
            ErrorCode.InvalidCredentials => StatusCodes.Status401Unauthorized,
            ErrorCode.AccessDenied => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status400BadRequest
        };

        return controller.Problem(
            detail: error.Message,
            title: errorCode?.ToString(),
            statusCode: statusCode
        );
    }

}

