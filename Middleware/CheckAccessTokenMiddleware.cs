using TwoCoinApi.Models;
using TwoCoinApi.Services;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TwoCoinApi
{
    public class CheckAccessTokenMiddleware
    {
        // Lưu middlewware tiếp theo trong Pipeline
        private readonly RequestDelegate _next;

        public CheckAccessTokenMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task Invoke(HttpContext httpContext, TokenManager tokenManager)
        {

            //Đến bước này là đã Authorize thành công rồi. Do khai báo bên startup sau middleware Authorize
            var endpoint = httpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(httpContext);
                return;
            }

            var accessToken = httpContext.Request.Headers["Authorization"].ToString().Split(" ").Last();
            var userId = new JwtSecurityTokenHandler().ReadJwtToken(accessToken).Payload?.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier).Value;

            try
            {
                if (tokenManager.IsExitsAccessToken(userId, accessToken))
                {
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
                await _next(httpContext);
            }
            catch
            {

                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return;
            }


        }
    }
}
