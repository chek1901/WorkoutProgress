using AWO.Services.Interfaces;
using AWO.ViewModels.Admin;
using AWO.ViewModels.Admin.Roles;
using AwoAppServices.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWO.Services
{
    public enum RoleCreation
    {
        Success,
        CreationError,
        RoleExist
    }
    public class AdminService : IAdminService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly GymadminContext context;

        public AdminService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
            GymadminContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.context = context;
        }

        public async Task<RoleCreation> CreateRole(string roleName)
        {
            //If role doesn't exist
            if (!context.Roles.Any(r => r.Name == roleName))
            {
                var newRole = new IdentityRole
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                };
                var result = await roleManager.CreateAsync(newRole);
                if (result.Succeeded)
                {
                    await context.SaveChangesAsync();
                    return RoleCreation.Success;
                }
                else
                {
                    return RoleCreation.CreationError;
                }
            }

            //If role exists
            return RoleCreation.RoleExist;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersInrole(string roleName)
        {
            IEnumerable<ApplicationUser> usersInRole = 
                await userManager.GetUsersInRoleAsync(roleName);
            
            return usersInRole; 
        }

        public async Task<IEnumerable<RoleObject>> GetRoles()
        {
            var roleList = new List<RoleObject>();
            var allRoles = roleManager.Roles;

            foreach (var role in allRoles)
            {
                var roleObject = new RoleObject
                {
                    Name = role.Name,
                    RoleId = role.Id,
                    Users = await GetUsersInrole(role.Name)
                };
                roleList.Add(roleObject);
            }
            return roleList.OrderBy(x => x.Name);
        }
    }
}
