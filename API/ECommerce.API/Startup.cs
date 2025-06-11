using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using ECommerce.BLL.DTO;
using ECommerce.BLL.Features.Users.Filter;
using ECommerce.BLL.Injector;
using ECommerce.BLL.Middleware;
using ECommerce.BLL.Validators;
using ECommerce.Core.Helpers;
using ECommerce.Core.Middlwares;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using FluentValidation.AspNetCore;
using Localization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sortech.CRM.Identity.Api.Localization;

namespace ECommerce.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            //****************** Cors ******************************
            services.AddCors();

            //****************** Fluent Validation ******************************
            services.AddFluentValidation(fluent =>
                fluent.RegisterValidatorsFromAssemblyContaining<BaseValidator>()
            );

            //****************** Controllers ******************************
            services.AddControllers();

            //****************** Newtonsoft Json ******************************
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
                        .Json
                        .ReferenceLoopHandling
                        .Ignore
                );

            //****************** Swagger ******************************
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Commerce.API", Version = "v1" });
                c.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = @"JWT Authorization header using the Bearer scheme",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer"
                    }
                );
                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Path,
                            },
                            new List<string>()
                        }
                    }
                );
            });

            //****************** Application DB context ******************************
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
            //);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            //****************** Identity Setting ******************************
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services
                .AddIdentity<User, Role>(Option =>
                {
                    Option.Password.RequireNonAlphanumeric = false;
                    Option.Password.RequireDigit = false;
                    Option.Password.RequiredUniqueChars = 0;
                    Option.Password.RequireLowercase = false;
                    Option.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //****************** JWT ******************************
            services.Configure<JWTHelpers>(Configuration.GetSection("JWT"));
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidAudience = Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["JWT:Key"])
                        )
                    };
                });

            //****************** Auto Mapper ******************************
            services.AddAutoMapper(typeof(MappingProfile));

            //****************** Http Context Accessor ******************************
            services.AddHttpContextAccessor();

            //****************** Localization ******************************
            services.AddLocalization();
            services.AddSingleton<LocalizerMiddleware>();
            services.AddDistributedMemoryCache();
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

            Injector.InjectServices(services, Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //*****************  Global Error Handling  ***************************
            app.ConfigureExceptionHandler();

            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.EnablePersistAuthorization();
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce.API v1");
                    c.DisplayRequestDuration();
                    c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
                });
            }

            //****************** Localization ******************************
            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(new CultureInfo("ar-EG"))
            };
            app.UseRequestLocalization(options);
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(env.ContentRootPath, "Images")
                    ),
                    RequestPath = "/Images",
                    ServeUnknownFileTypes = true
                }
            );
            app.UseMiddleware<LocalizerMiddleware>();
            app.UseCors(builder =>
                builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials()
            );
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<JwtAuthenticationMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
