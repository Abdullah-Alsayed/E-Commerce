using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Core
{
    public static class DataSeeder
    {
        public static void SeedData(Applicationdbcontext context)
        {
            var systemUser = Guid.NewGuid().ToString();
            if (!context.Users.Any(x => x.UserName == "System"))
            {
                context.Users.Add(
                    new User
                    {
                        Id = systemUser,
                        UserName = "System",
                        FirstName = "System",
                        LastName = "System",
                        Address = "System",
                        Email = "System"
                    }
                );
            }
            else
                systemUser = context.Users.FirstOrDefault(x => x.UserName == "System").Id;

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
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new List<IdentityRole>
                    {
                        new IdentityRole
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = Constants.Roles.Admin,
                            NormalizedName = Constants.Roles.Admin.ToUpper(),
                            ConcurrencyStamp = Guid.NewGuid().ToString()
                        },
                        new IdentityRole
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = Constants.Roles.User,
                            NormalizedName = Constants.Roles.User.ToUpper(),
                            ConcurrencyStamp = Guid.NewGuid().ToString()
                        }
                    }
                );
            }
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
            if (!context.Statuses.Any())
            {
                context.Statuses.Add(
                    new Status
                    {
                        NameAR = "مكتمل",
                        NameEN = "Complete",
                        IsCompleted = true,
                        Order = 5,
                        CreateBy = systemUser,
                    }
                );
            }
            context.SaveChanges();
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
