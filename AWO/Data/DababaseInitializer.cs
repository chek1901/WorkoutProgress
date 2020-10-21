using AWO.Services;
using AwoAppServices.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace AWO.Data
{

    public class DatabaseInitializer
    {
        //public async Task Initialize(GymadminContext context,
        //                UserManager<ApplicationUser> userManager,
        //                RoleManager<IdentityRole> roleManager)
        //{
        //    context.Database.Migrate();

        //    await SeedIdentityRoles(roleManager, context);
        //    await SeedUsersWithRoles(userManager, roleManager, context);
        //}

        //public async Task SeedIdentityRoles(RoleManager<IdentityRole> roleManager, GymadminContext context)
        //{

        //    if (!context.Roles.Any(r => r.Name == "Admin"))
        //    {
        //        var newRole = new IdentityRole
        //        {
        //            Name = "Admin",
        //            NormalizedName = "ADMIN"
        //        };
        //        await roleManager.CreateAsync(newRole);
        //    }
        //    if (!context.Roles.Any(r => r.Name == "User"))
        //    {
        //        var newRole = new IdentityRole
        //        {
        //            Name = "User",
        //            NormalizedName = "USER"
        //        };
        //        await roleManager.CreateAsync(newRole);
        //    }
        //}

        //public async Task SeedUsersWithRoles(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        //    GymadminContext context)
        //{

        //    var adminRole = await roleManager.Roles.SingleOrDefaultAsync(x => x.Name == "Admin");
        //    if (!context.Users.Any(u => u.Email == "admin@gym.se"))
        //    {
               
        //        var user = new ApplicationUser
        //        {
        //            UserName = "Administrator",
        //            Email = "Admin@gym.se",
        //            EmailConfirmed = true,
        //            GymUserId = await GetGymUserId("admin@gym.se", context),
        //            RoleName = adminRole.Name
                    
        //        };
        //        await userManager.CreateAsync(user, "Password@1");
        //        await userManager.AddToRoleAsync(user, "Admin");
        //    }
        //    //var loggedInUser = await context.Users.SingleOrDefaultAsync(u => u.UserName == "Administrator");
            
        //    if (!context.Users.Any(u => u.Email == "user@gym.se"))
        //    {
        //        var userRole = await roleManager.Roles.SingleOrDefaultAsync(x => x.Name == "User");

        //        var user = new ApplicationUser
        //        {
        //            UserName = "User",
        //            Email = "User@gym.se",
        //            EmailConfirmed = true,
        //            GymUserId = await GetGymUserId("user@gym.se", context),
        //            RoleName = userRole.Name

        //        };
        //        await userManager.CreateAsync(user, "Password@2");
        //        await userManager.AddToRoleAsync(user, "User");
        //    }
        //}

        //private async Task<int> GetGymUserId(string email, GymadminContext context)
        //{
        //    var gymUser = new GymUsers
        //    {
        //        Email = email
        //    };
        //    context.GymUsers.Add(gymUser);
        //    await context.SaveChangesAsync();

        //    var newlyCreatedGymUser = await context.GymUsers.SingleOrDefaultAsync(user => user.Email == email);
        //    return newlyCreatedGymUser.GymUserId;

        //}
    }
}
