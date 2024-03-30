using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ECommerce.BL.Repository;
using ECommerce.BLL.DTO;
using ECommerce.BLL.Features.Areas.Services;
using ECommerce.BLL.Features.Brands.Services;
using ECommerce.BLL.Features.Carts.Services;
using ECommerce.BLL.Features.Categories.Services;
using ECommerce.BLL.Features.Colors.Services;
using ECommerce.BLL.Features.ContactUses.Services;
using ECommerce.BLL.Features.Expenses.Services;
using ECommerce.BLL.Features.Feedbacks.Services;
using ECommerce.BLL.Features.Governorates.Services;
using ECommerce.BLL.Features.Invoices.Services;
using ECommerce.BLL.Features.Orders.Services;
using ECommerce.BLL.Features.Products.Services;
using ECommerce.BLL.Features.Reviews.Services;
using ECommerce.BLL.Features.Roles.Services;
using ECommerce.BLL.Features.Settings.Services;
using ECommerce.BLL.Features.Sizes.Services;
using ECommerce.BLL.Features.Sliders.Services;
using ECommerce.BLL.Features.Statuses.Services;
using ECommerce.BLL.Features.Stocks.Services;
using ECommerce.BLL.Features.SubCategories.Services;
using ECommerce.BLL.Features.Units.Services;
using ECommerce.BLL.Features.Vendors.Services;
using ECommerce.BLL.Features.Vouchers.Services;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Repository;
using ECommerce.BLL.Validators;
using ECommerce.Core;
using ECommerce.Core.Services.MailServices;
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
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

            //****************** Services ******************************
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            #region Feature Services
            services.AddScoped<IAreaService, AreaService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<ISizeService, SizeService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IColorService, ColorService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<IMailServicies, MailServicies>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IMailServicies, MailServicies>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IContactUsService, ContactUsService>();
            services.AddScoped<ISubCategoryService, SubCategoryService>();
            services.AddScoped<IGovernorateService, GovernorateService>();
            #endregion
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            //****************** Data Seeder ******************************
            using var scope = app
                .ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                await DataSeeder.SeedData(context, roleManager);
            }

            //****************** Localization ******************************
            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(new CultureInfo("ar-EG"))
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
