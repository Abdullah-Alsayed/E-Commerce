using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ECommerce.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.BLL.Middleware
{
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public JwtAuthenticationMiddleware(
            RequestDelegate next,
            IServiceScopeFactory serviceScopeFactory
        )
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (
                    context.Request.Headers.TryGetValue("Authorization", out var authorizationToken)
                )
                {
                    var token = authorizationToken.ToString().Replace("Bearer ", "");
                    var IsTokenExpired = dbContext.TokenExpired.Any(x =>
                        x.Token == token.ToString()
                    );
                    if (IsTokenExpired)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
