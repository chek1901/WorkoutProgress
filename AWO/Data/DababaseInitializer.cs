using AwoAppServices.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AWO.Data
{

    public class DatabaseInitializer
    {
        public async Task Initialize(GymadminContext context,
                        UserManager<ApplicationUser> userManager,
                        RoleManager<ApplicationRole> roleManager)
        {
            context.Database.Migrate();

            await SeedApplicationRoles(roleManager, context);
            await SeedUsersWithRoles(userManager, roleManager, context);
        }

        public async Task SeedApplicationRoles(RoleManager<ApplicationRole> roleManager, GymadminContext context)
        {

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var newRole = new ApplicationRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    OriginDate = DateTime.Now
                };
                await roleManager.CreateAsync(newRole);
            }
            if (!context.Roles.Any(r => r.Name == "User"))
            {
                var newRole = new ApplicationRole
                {
                    Name = "User",
                    NormalizedName = "USER",
                    OriginDate = DateTime.Now

                };
                await roleManager.CreateAsync(newRole);
            }
            if (!context.Roles.Any(r => r.Name == "Premium"))
            {
                var newRole = new ApplicationRole
                {
                    Name = "Premium",
                    NormalizedName = "PREMIUM",
                    OriginDate = DateTime.Now

                };
                await roleManager.CreateAsync(newRole);
            }
        }

        public async Task SeedUsersWithRoles(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            GymadminContext context)
        {

            var adminRole = await roleManager.Roles.SingleOrDefaultAsync(x => x.Name == "Admin");
            if (!context.Users.Any(u => u.Email == "admin@gym.se"))
            {

                var user = new ApplicationUser
                {
                    UserName = "Administrator",
                    Email = "Admin@gym.se",
                    EmailConfirmed = true,
                    GymUserId = await GetGymUserId("admin@gym.se", context),
                    RoleName = adminRole.Name

                };
                await userManager.CreateAsync(user, "Password@1");
                await userManager.AddToRoleAsync(user, "Admin");
            }
            //var loggedInUser = await context.Users.SingleOrDefaultAsync(u => u.UserName == "Administrator");

            if (!context.Users.Any(u => u.Email == "user@gym.se"))
            {
                var userRole = await roleManager.Roles.SingleOrDefaultAsync(x => x.Name == "User");

                var user = new ApplicationUser
                {
                    UserName = "User",
                    Email = "User@gym.se",
                    EmailConfirmed = true,
                    RoleName = userRole.Name,
                    GymUserId = await GetGymUserId("user@gym.se", context)

                };
                await userManager.CreateAsync(user, "Password@2");
                await userManager.AddToRoleAsync(user, "User");
            }
            if (!context.Users.Any(u => u.Email == "premium@gym.se"))
            {
                var userRole = await roleManager.Roles.SingleOrDefaultAsync(x => x.Name == "Premium");

                var user = new ApplicationUser
                {
                    UserName = "PremiumUser",
                    Email = "premium@gym.se",
                    EmailConfirmed = true,
                    RoleName = userRole.Name,
                    GymUserId = await GetGymUserId("premium@gym.se", context)

                };
                await userManager.CreateAsync(user, "Password@2");
                await userManager.AddToRoleAsync(user, "Premium");
            }
        }

        private async Task<int> GetGymUserId(string email, GymadminContext context)
        {
            var gymUser = new GymUsers
            {
                Email = email
            };
            context.GymUsers.Add(gymUser);
            await context.SaveChangesAsync();

            var newlyCreatedGymUser = await context.GymUsers.FirstOrDefaultAsync(user => user.Email == email);

            return newlyCreatedGymUser.GymUserId;
        }
    }
}
