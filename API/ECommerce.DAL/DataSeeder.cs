using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Core.PermissionsClaims;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Identity;
using static ECommerce.Core.Constants;

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
            await SeedRoles(context, roleManager);
            string systemUser = await GetUser(context, userManager);
            SeedGovernorates(context, systemUser);
            SeedSettings(context, systemUser);
            SeedStatuses(context, systemUser);
            context.SaveChanges();
        }

        private static async Task<string> GetUser(
            ApplicationDbContext context,
            UserManager<User> userManager
        )
        {
            var systemUser = Guid.NewGuid().ToString();
            if (!context.Users.Any(x => x.UserName == "System"))
            {
                var adminUser = new User
                {
                    Id = systemUser,
                    UserName = "System",
                    FirstName = "System",
                    LastName = "System",
                    Address = "System",
                    Email = "System",
                    EmailConfirmed = true,
                    CreateAt = DateTime.UtcNow
                };
                await userManager.CreateAsync(adminUser, "P@ssword123");
                await userManager.AddToRoleAsync(adminUser, Constants.Roles.SuperAdmin);
            }
            else
                systemUser = context.Users.FirstOrDefault(x => x.UserName == "System").Id;
            return systemUser;
        }

        private static void SeedStatuses(ApplicationDbContext context, string systemUser)
        {
            if (!context.Statuses.Any())
            {
                context.Statuses.Add(
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

        private static void SeedSettings(ApplicationDbContext context, string systemUser)
        {
            if (!context.Settings.Any())
            {
                context.Settings.Add(
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
            if (!context.Roles.Any())
            {
                var SuperAdmin = new Role
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Constants.Roles.SuperAdmin,
                    Description = Constants.Roles.SuperAdmin,
                    NormalizedName = Constants.Roles.SuperAdmin.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    CreateAt = DateTime.UtcNow,
                    IsDefault = true
                };
                var user = new Role
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Constants.Roles.User,
                    Description = Constants.Roles.User,
                    NormalizedName = Constants.Roles.User.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    CreateAt = DateTime.UtcNow,
                    IsDefault = true
                };
                await roleManager.CreateAsync(SuperAdmin);
                await roleManager.CreateAsync(user);
                await roleManager.AddPermissionClaim(SuperAdmin, Constants.EntityKeys.Product);
            }
        }

        private static void SeedGovernorates(ApplicationDbContext context, string systemUser)
        {
            if (!context.Governorates.Any())
            {
                string governoratesJson = File.ReadAllText("Governorates.json");
                string AreasJson = File.ReadAllText("Areas.json");
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
                context.Governorates.AddRange(governoratesList);
            }
        }

        private static async Task AddPermissionClaim(
            this RoleManager<Role> roleManager,
            Role role,
            string module
        )
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModule(module);
            foreach (var permission in allPermissions)
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                    await roleManager.AddClaimAsync(
                        role,
                        new Claim(Constants.Permission, permission)
                    );
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
