using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Core.PermissionsClaims;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Core
{
    public static class DataSeeder
    {
        public static async Task SeedData(
            ApplicationDbContext context,
            RoleManager<Role> roleManager,
            UserManager<User> userManager
        )
        {
            try
            {
                await SeedRoles(context, roleManager);
                string systemUser = await GetUser(context, userManager);
                await SeedGovernorates(context, systemUser);
                await SeedSettings(context, systemUser);
                await SeedStatuses(context, systemUser);
                await context.SaveChangesAsync();
            }
            catch (global::System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task<string> GetUser(
            ApplicationDbContext context,
            UserManager<User> userManager
        )
        {
            var systemUser = Guid.NewGuid().ToString();
            var hasDefault = await context.Users.AnyAsync(x => x.UserName == "System");
            if (!hasDefault)
            {
                var adminUser = new User
                {
                    Id = systemUser,
                    UserName = Constants.System,
                    FirstName = Constants.System,
                    LastName = Constants.System,
                    Address = Constants.System,
                    Email = Constants.System,
                    EmailConfirmed = true,
                    Photo = $"/Images/{Constants.PhotoFolder.User}/{Constants.DefaultPhotos.User}",
                    CreateAt = DateTime.UtcNow
                };
                await userManager.CreateAsync(adminUser, "P@ssword123");
                await userManager.AddToRoleAsync(adminUser, Constants.Roles.SuperAdmin);
            }
            else
            {
                var defaultUser = await context.Users.FirstOrDefaultAsync(x =>
                    x.UserName == "System"
                );
                systemUser = defaultUser.Id;
            }
            return systemUser;
        }

        private static async Task SeedStatuses(ApplicationDbContext context, string systemUser)
        {
            var hasStatuses = await context.Statuses.AnyAsync();
            if (!hasStatuses)
            {
                await context.Statuses.AddAsync(
                    new Status
                    {
                        NameAR = "مكتمل",
                        NameEN = "Complete",
                        Order = 10,
                        CreateBy = systemUser,
                    }
                );
            }
        }

        private static async Task SeedSettings(ApplicationDbContext context, string systemUser)
        {
            var hasSettings = await context.Settings.AnyAsync();
            if (!hasSettings)
            {
                await context.Settings.AddAsync(
                    new Setting
                    {
                        Address = "Dummy Data",
                        Email = "test@test.com",
                        FaceBook = "FaceBook.com",
                        Instagram = "Instagram.com",
                        MainColor = "#4990e2",
                        Logo = "Logo.png",
                        Phone = "011111111111",
                        Whatsapp = "whatsapp.com",
                        Youtube = "Youtube.com",
                        Title = "Dummy Data",
                        CreateBy = systemUser,
                    }
                );
            }
        }

        private static async Task SeedRoles(
            ApplicationDbContext context,
            RoleManager<Role> roleManager
        )
        {
            var hasRoles = await context.Roles.AnyAsync();
            if (!hasRoles)
            {
                var SuperAdmin = new Role
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Constants.Roles.SuperAdmin,
                    Description = Constants.Roles.SuperAdmin,
                    NormalizedName = Constants.Roles.SuperAdmin.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    CreateAt = DateTime.UtcNow,
                    IsMaster = true
                };

                await roleManager.CreateAsync(SuperAdmin);
                await AddSuperAdminRoleClaim(context, SuperAdmin);
                await roleManager.CreateAsync(
                    new Role
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = Constants.Roles.User,
                        Description = Constants.Roles.User,
                        NormalizedName = Constants.Roles.User.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        CreateAt = DateTime.UtcNow,
                        IsMaster = false
                    }
                );
                await roleManager.CreateAsync(
                    new Role
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = Constants.Roles.Client,
                        Description = Constants.Roles.Client,
                        NormalizedName = Constants.Roles.Client.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        CreateAt = DateTime.UtcNow,
                        IsMaster = false
                    }
                );
            }
        }

        private static async Task SeedGovernorates(ApplicationDbContext context, string systemUser)
        {
            var hasGovernorates = await context.Governorates.AnyAsync();
            if (!hasGovernorates)
            {
                string governoratesJson = await File.ReadAllTextAsync("Governorates.json");
                string AreasJson = await File.ReadAllTextAsync("Areas.json");
                var governorates = JsonSerializer.Deserialize<List<GovernorateJson>>(
                    governoratesJson
                );
                var Areas = JsonSerializer.Deserialize<List<AreaJson>>(AreasJson);

                var governoratesList = governorates.Select(governorate => new Governorate
                {
                    ID = Guid.NewGuid(),
                    NameAR = governorate.NameAR,
                    NameEN = governorate.NameEN,
                    Areas = Areas
                        .Where(x => x.GovernorateID == governorate.id)
                        .Select(area => new Area
                        {
                            NameAR = area.NameAR,
                            NameEN = area.NameEN,
                            CreateBy = systemUser
                        })
                        .ToList(),
                    CreateBy = systemUser,
                });
                await context.Governorates.AddRangeAsync(governoratesList);
            }
        }

        private static async Task AddSuperAdminRoleClaim(ApplicationDbContext context, Role role)
        {
            var allPermissions = Permissions.GetAllPermissions();
            var claims = allPermissions
                .Select(claim => new RoleClaims
                {
                    ClaimType = Constants.Permission,
                    ClaimValue = claim.Claim,
                    Module = claim.Module,
                    Operation = claim.Operation,
                    RoleId = role.Id,
                })
                .ToList();
            await context.RoleClaims.AddRangeAsync(claims);
        }
    }
}

public class AreaJson
{
    public string id { get; set; }
    public string GovernorateID { get; set; }
    public string NameAR { get; set; }
    public string NameEN { get; set; }
}

public class GovernorateJson
{
    public string id { get; set; }
    public string NameAR { get; set; }
    public string NameEN { get; set; }
}
