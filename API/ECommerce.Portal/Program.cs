using System.Globalization;
using ECommerce.BLL.DTO;
using ECommerce.BLL.Features.Users.Filter;
using ECommerce.BLL.Injector;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using LocalizationAPI.Localization;
using MasBeach.BackEnd.Portal.Localization.LocalizationManger;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Authentication Configuration
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";
    });

//****************** Application DB context ******************************
//services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
//);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//****************** Identity Setting ******************************
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder
    .Services.AddIdentity<User, ECommerce.DAL.Entity.Role>(Option =>
    {
        Option.Password.RequireNonAlphanumeric = false;
        Option.Password.RequireDigit = false;
        Option.Password.RequiredUniqueChars = 0;
        Option.Password.RequireLowercase = false;
        Option.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//****************** Auto Mapper ******************************
builder.Services.AddAutoMapper(typeof(MappingProfile));

//****************** Http Context Accessor ******************************
builder.Services.AddHttpContextAccessor();

Injector.InjectServices(builder.Services, builder.Configuration);

//****************** Localization ******************************
builder.Services.AddLocalization();
builder.Services.AddSingleton<LocalizerMiddleware>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.AddScoped<ILocalizationManger, LocalizationManger>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
} //****************** Localization ******************************
var options = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(new CultureInfo("ar-EG"))
};
app.UseRequestLocalization(options);
app.UseMiddleware<LocalizerMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
