using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MonaDotNetTemplate.Utilities;
using MonaDotNetTemplate.Extensions;

namespace MonaDotNetTemplate.API.Extensions
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<ControllerBase> _logger;


        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ControllerBase> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(Microsoft.AspNetCore.Http.HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException error)
            {
                context.Response.ContentType = "application/json";
                var result = ResponseBuilder.Instance.Build(error, LoginContext.Instance?.CurrentUser?.Language??"vi");
                context.Response.StatusCode = result.StatusCode;
                await context.Response.WriteAsync(result.ToString());
            }
            catch (Exception error)
            {
                context.Response.ContentType = "application/json";
                var result = new AppDomainResult()
                {
                    Message = error.Message,
                    StatusCode = 500
                };
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(result.ToString());
            }
        }
    }
}
