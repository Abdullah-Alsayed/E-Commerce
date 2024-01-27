using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ECommerce.Helpers
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

                var governoratesList = governorates.Select(
                    governorate =>
                        new Governorate
                        {
                            ID = Guid.NewGuid(),
                            NameAR = governorate.NameAR,
                            NameEN = governorate.NameEN,
                            Areas = Areas
                                .Where(x => x.GovernorateID == governorate.id)
                                .Select(
                                    area =>
                                        new Area
                                        {
                                            NameAR = area.NameAR,
                                            NameEN = area.NameEN,
                                            CreateBy = systemUser
                                        }
                                )
                                .ToList(),
                            CreateBy = systemUser,
                        }
                );
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
