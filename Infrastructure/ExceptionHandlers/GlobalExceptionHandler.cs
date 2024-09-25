using Domain.Constants.Enums;
using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

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
                //{ typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                //{ typeof(ForbiddenAccessException), HandleForbiddenAccessException },
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
                await HandleSystemException(httpContext, exception);
            }
            return false;
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
            //problemDetails.Extensions.Add("traceId", Activity.Current.GetTraceId());

            response.ContentType = "application/problem+json";
            response.StatusCode = problemDetails.Status.Value;

            //var result = JsonSerializer.Serialize(problemDetails);
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
            //problemDetails.Extensions.Add("traceId", Activity.Current.GetTraceId());

            response.ContentType = "application/problem+json";
            response.StatusCode = problemDetails.Status.Value;

            //var result = JsonSerializer.Serialize(problemDetails);
            await response.WriteAsJsonAsync(problemDetails);
        }

        private async Task HandleSystemException(HttpContext httpContext, Exception exception)
        {

            var response = httpContext.Response;

            _logger.LogError(exception, "[{Ticks}-{ThreadId}]", DateTime.UtcNow.Ticks, Environment.CurrentManagedThreadId);

            if (_options.DetailLevel != GlobalExceptionDetailLevel.Throw)
            {            

                var problemDetails = new ProblemDetails
                {
                    Detail = _options.GetErrorMessage(exception),
                    Instance = null,
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Internal Server Error",
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
    }
}
