using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MonaDotNetTemplate.Extensions;
using MonaDotNetTemplate.Services.Interfaces.Auth;
using MonaDotNetTemplate.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.API.Extensions
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        private readonly ITokenManagerService tokenManagerService;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, ITokenManagerService tokenManagerService)
        {
            _next = next;
            _appSettings = appSettings.Value;
            this.tokenManagerService = tokenManagerService;
        }

        public async Task Invoke(Microsoft.AspNetCore.Http.HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (await tokenManagerService.IsCurrentActiveToken())
            {
                try
                {
                    if (token != null)
                        attachUserToContext(context, token);
                    await _next(context);
                }
                catch
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    var result = ResponseBuilder.Instance.Build(new AppException(CoreContant.ResponseMessageType.Unauthenticaion), LoginContext.Instance?.CurrentUser?.Language ?? "vi");
                    await context.Response.WriteAsync(result.ToString());
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                var result = ResponseBuilder.Instance.Build(new AppException(CoreContant.ResponseMessageType.Unauthenticaion), LoginContext.Instance?.CurrentUser?.Language?? "vi");
                await context.Response.WriteAsync(result.ToString());
            }
        }

        private void attachUserToContext(Microsoft.AspNetCore.Http.HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userModel = new UserLoginRequest();
                var claim = jwtToken.Claims.First(x => x.Type == ClaimTypes.UserData);
                if (claim != null)
                {
                    userModel = JsonConvert.DeserializeObject<UserLoginRequest>(claim.Value);
                }
                var expirationTime = jwtToken.ValidTo;
                context.Items["User"] = userModel;
            }
            catch (Exception ex)
            {
                throw new AppException(CoreContant.ResponseMessageType.Unauthenticaion);
            }
        }
    }
}
