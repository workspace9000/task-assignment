using GuardNet;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskAssignment.Api.Shared;
using TaskAssignment.Domain;
using TaskAssignment.Domain.Exceptions;
using TaskAssignment.Infrastructure.Shared;

namespace TaskAssignment.Api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
[Route("api")]
public class ErrorController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public ErrorController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [Route("error")]
    public ErrorResultDto? Error()
    {
        var errorContext = HttpContext.Features.Get<IExceptionHandlerFeature>();

        Guard.For<UnexpectedStateException>(() => errorContext == null, $"Incorrect  usage");

        var exception = errorContext!.Error;

        if (exception is TaskCanceledException)
        {
            return null;
        }


        var handlerName = HttpContext.Items["HandlerName"]?.ToString() ?? "N/A";
        var executionContextId = HttpContext.TraceIdentifier;

        SetStatusCode(exception);

        var errorDto = ErrorResultDtoFactory.Create(exception, executionContextId);

        if (_env.IsNotRestricted())
        {
            errorDto.AddTechDetails(new TechDetails { MethodName = handlerName, Exception = exception.ToString(), InnerException = exception.InnerException?.ToString() });
        }
        else
        {
            errorDto.ClearTechDetails();
        }

        return errorDto;
    }

    private void SetStatusCode(Exception exception)
    {
        Response.StatusCode = (int)HttpStatusCode.BadRequest;

        if (exception is UserException userExeption)
        {
            switch (userExeption.ErrorCode)
            {
                case TaskAssignmentErrorCodes.NotAuthorized:
                case TaskAssignmentErrorCodes.PermissionDenied:
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;
                default:
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
            }
        }
    }
}
