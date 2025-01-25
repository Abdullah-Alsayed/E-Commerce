using System;
using System.Net;
using System.Text.Json;
using ECommerce.BLL.DTO;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Core.Middlwares
{
    public static class ExceptionMiddlewareExtensions
    {
        //*** Configure Global Exception Handler
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        //Save To DB
                        var scope = app.ApplicationServices.CreateScope();
                        var dbContext =
                            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        dbContext.ErrorLogs.Add(
                            new ErrorLog
                            {
                                Endpoint = contextFeature.Path,
                                Source = contextFeature.Error.Source,
                                StackTrace = JsonSerializer.Serialize(
                                    contextFeature.Error.StackTrace
                                ),
                                Message = contextFeature.Error.Message,
                                Date = DateTime.Now
                            }
                        );
                        dbContext.SaveChanges();
                        await context.Response.WriteAsync(
                            new ResponseErrorLogDto()
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = "Internal Server Error.",
                            }.ToString()
                        );
                    }
                });
            });
        }
    }
}
