using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using IdentityServer_Mongo.Entities;

namespace IdentityServer_Mongo.Helpers
{
    public static class Roles
    {
        public const string API_IDENTITY = "API_Identity";
        public const string API_DOSSIER = "API_Dossier";
    }

    public class SeedRoles
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            // Create roles and seed them to database
            if (!await RoleManager.RoleExistsAsync(Roles.API_IDENTITY))
                await RoleManager.CreateAsync(new ApplicationRole(Roles.API_IDENTITY));
            if (!await RoleManager.RoleExistsAsync(Roles.API_DOSSIER))
                await RoleManager.CreateAsync(new ApplicationRole(Roles.API_DOSSIER));
        }
    }
}