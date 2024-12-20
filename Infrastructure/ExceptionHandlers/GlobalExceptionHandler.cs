﻿using Domain.Constants.Enums;
using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace Infrastructure.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly GlobalExceptionHandlerOptions _options;
        private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IOptions<GlobalExceptionHandlerOptions> options)
        {
            _logger = logger;
            _options = options.Value;
            _exceptionHandlers = new()
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(UnauthorizedAccessException), HandleUnAuthorizeException },
                { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
                { typeof(BadRequestException), HandleBadRequestException },
                { typeof(ConflictException), HandleConflictException },
            };
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var exceptionType = exception.GetType();

            if (_exceptionHandlers.ContainsKey(exceptionType))
            {
                await _exceptionHandlers[exceptionType].Invoke(httpContext, exception);
                return true;
            }
            else
            {
                await HandleUnhandleException(httpContext, exception);
                return true;
            }
        }

        private async Task HandleNotFoundException(HttpContext httpContext, Exception exception)
        {
            var response = httpContext.Response;
            var problemDetails = new ProblemDetails
            {
                Detail = exception.Message,
                Instance = null,
                Status = (int)HttpStatusCode.NotFound,
                Title = "Not Found",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4"
            };

            problemDetails.Extensions.Add("message", exception.Message);

            response.ContentType = "application/problem+json";
            response.StatusCode = problemDetails.Status.Value;

            await response.WriteAsJsonAsync(problemDetails);
        }

        private async Task HandleBadRequestException(HttpContext httpContext, Exception exception)
        {
            var response = httpContext.Response;
            var problemDetails = new ProblemDetails
            {
                Detail = exception.Message ?? "Bad request",
                Instance = null,
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Bad Request",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
            };

            problemDetails.Extensions.Add("message", exception.Message);

            response.ContentType = "application/problem+json";
            response.StatusCode = problemDetails.Status.Value;

            await response.WriteAsJsonAsync(problemDetails);
        }

        private async Task HandleValidationException(HttpContext httpContext, Exception exception)
        {
            var ex = (ValidationException)exception;

            var response = httpContext.Response;

            var problemDetails = new ValidationProblemDetails(ex.Errors)
            {
                Detail = ex.Message,
                Instance = null,
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Bad Request",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
            };

            problemDetails.Extensions.Add("message", ex.Message);

            response.ContentType = "application/problem+json";
            response.StatusCode = problemDetails.Status.Value;

            await response.WriteAsJsonAsync(problemDetails);
        }

        private async Task HandleUnhandleException(HttpContext httpContext, Exception exception)
        {

            var response = httpContext.Response;

            _logger.LogError(exception, "[{Ticks}-{ThreadId}]", DateTime.UtcNow.Ticks, Environment.CurrentManagedThreadId);

            if (_options.DetailLevel != GlobalExceptionDetailLevel.Throw)
            {

                var problemDetails = new ProblemDetails
                {
                    Detail = "An unexpected error occurred",
                    Instance = null,
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Internal Server Error",
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
                };

                problemDetails.Extensions.Add("message", exception.Message);
                // problemDetails.Extensions.Add("traceId", Activity.Current.GetTraceId());

                response.ContentType = "application/problem+json";
                response.StatusCode = problemDetails.Status.Value;

                await response.WriteAsJsonAsync(problemDetails);
            }


        }
        private async Task HandleConflictException(HttpContext httpContext, Exception exception)
        {
            var response = httpContext.Response;
            var problemDetails = new ProblemDetails
            {
                Detail = exception.Message,
                Instance = null,
                Status = (int)HttpStatusCode.Conflict,
                Title = "Conflict",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8"
            };

            //problemDetails.Extensions.Add("traceId", Activity.Current.GetTraceId());

            response.ContentType = "application/problem+json";
            response.StatusCode = problemDetails.Status.Value;

            await response.WriteAsJsonAsync(problemDetails);
        }

        private async Task HandleMediaException(HttpContext context, Exception exception)
        {
            var response = context.Response;

            _logger.LogError(exception, "[{Ticks}-{ThreadId}]", DateTime.UtcNow.Ticks, Environment.CurrentManagedThreadId);

            if (_options.DetailLevel != GlobalExceptionDetailLevel.Throw)
            {

                var problemDetails = new ProblemDetails
                {
                    Detail = _options.GetErrorMessage(exception),
                    Instance = null,
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "Media Error",
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
                };

                problemDetails.Extensions.Add("message", _options.GetErrorMessage(exception));
                // problemDetails.Extensions.Add("traceId", Activity.Current.GetTraceId());

                response.ContentType = "application/problem+json";
                response.StatusCode = problemDetails.Status.Value;

                //var result = JsonSerializer.Serialize(problemDetails);
                await response.WriteAsJsonAsync(problemDetails);
            }
        }

        private async Task HandleUnAuthorizeException(HttpContext httpContext, Exception exception)
        {
            var response = httpContext.Response;
            var problemDetails = new ProblemDetails
            {
                Detail = exception.Message ?? "Access Denied",
                Instance = null,
                Status = (int)HttpStatusCode.Unauthorized,
                Title = "Unauthorized",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.5"
            };

            problemDetails.Extensions.Add("message", exception.Message);

            response.ContentType = "application/problem+json";
            response.StatusCode = problemDetails.Status.Value;

            await response.WriteAsJsonAsync(problemDetails);
        }

        private async Task HandleForbiddenAccessException(HttpContext context, Exception exception)
        {
            var response = context.Response;
            var problemDetails = new ProblemDetails
            {
                Detail = exception.Message ?? "Forbidden",
                Instance = null,
                Status = (int)HttpStatusCode.Forbidden,
                Title = "Forbidden",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3"
            };

            problemDetails.Extensions.Add("message", exception.Message);

            response.ContentType = "application/problem+json";
            response.StatusCode = problemDetails.Status.Value;

            await response.WriteAsJsonAsync(problemDetails);
        }
    }
}
