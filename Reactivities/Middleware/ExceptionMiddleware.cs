using Application.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Reactivities.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                HandleException(context, ex, _logger);
            }
        }

        private void HandleException(HttpContext context, Exception ex, ILogger<ExceptionMiddleware> logger)
        {
            Object error = null;
            switch (ex)
            {
                case RestException re:
                    logger.LogError(ex, "REST ERROR");
                    error = re.Error;
                    context.Response.StatusCode = (int)re.HttpStatusCode;
                    break;
                case Exception e:
                    logger.LogError(ex, "SERVER ERROR");
                    error = String.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";
            
            if(error != null)
            {
                var result = JsonSerializer.Serialize(error);
                context.Response.WriteAsync(result);
            }
        }
    }
}
