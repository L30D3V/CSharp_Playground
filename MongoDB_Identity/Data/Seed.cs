using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using MongoDB_Identity.Models;

namespace MongoDB_Identity.Data
{
    public class Seed
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            // Adding Custom Roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            // Creating roles and seeding them to database
            if (!(await RoleManager.RoleExistsAsync(Roles.ROLE_API_DOSSIER)))
                await RoleManager.CreateAsync(new ApplicationRole(Roles.ROLE_API_DOSSIER));
        }
    }
}
