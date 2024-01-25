using ECommerce.BLL.DTO;
using ECommerce.BLL.IRepository;
using ECommerce.BL.Repository;
using ECommerce.Services.MailServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ECommerce.DAL.Entity;
using ECommerce.DAL;
using FluentValidation.AspNetCore;
using System;
using ECommerce.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using ECommerce.BLL.Validators;
using Localization;
using Microsoft.Extensions.Localization;
using Sortech.CRM.Identity.Api.Localization;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace ECommerce.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JWTHelpers>(Configuration.GetSection("JWT"));
            services.AddFluentValidation(
                fluent => fluent.RegisterValidatorsFromAssemblyContaining<BaseValidator>()
            );

            services.AddControllers();

            services
                .AddControllers()
                .AddNewtonsoftJson(
                    options =>
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
                            .Json
                            .ReferenceLoopHandling
                            .Ignore
                );

            services.AddDbContext<Applicationdbcontext>(
                options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
            );
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

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services
                .AddIdentity<User, IdentityRole>(Option =>
                {
                    Option.Password.RequireNonAlphanumeric = false;
                    Option.Password.RequireDigit = false;
                    Option.Password.RequiredUniqueChars = 0;
                    Option.Password.RequireLowercase = false;
                    Option.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<Applicationdbcontext>()
                .AddDefaultTokenProviders();
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailServices, MailServicies>();
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
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddHttpContextAccessor();

            // -- Localization
            services.AddLocalization();
            services.AddSingleton<LocalizerMiddleware>();
            services.AddDistributedMemoryCache();
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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

            using (
                var scope = app.ApplicationServices
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope()
            )
            {
                var context = scope.ServiceProvider.GetRequiredService<Applicationdbcontext>();
                DataSeeder.SeedData(context);
            }
            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US"))
            };
            app.UseRequestLocalization(options);
            app.UseStaticFiles();
            app.UseMiddleware<LocalizerMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
