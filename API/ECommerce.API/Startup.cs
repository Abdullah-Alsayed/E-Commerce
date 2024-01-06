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

namespace ECommerce.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFluentValidation(
                fluent => fluent.RegisterValidatorsFromAssemblyContaining<Startup>()
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
            services.AddAuthentication();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce.API v1");
                    c.DisplayRequestDuration();
                });
            }

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
