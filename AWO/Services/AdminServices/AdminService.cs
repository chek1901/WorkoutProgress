using AWO.Services.Interfaces;
using AWO.ViewModels.Admin;
using AWO.ViewModels.Admin.Roles;
using AwoAppServices.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
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

    public enum UpdateUserEnum
    {
        Success,
        UpdateError,
        NoUserToUpdate
    }

    public class AdminService : IAdminService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GymadminContext _context;

        public AdminService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager,
            GymadminContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<RoleCreation> CreateRole(string roleName)
        {
            if (!_context.Roles.Any(r => r.Name == roleName))
            {
                var newRole = new ApplicationRole
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper(),
                    OriginDate = DateTime.Now
                };

                var result = await _roleManager.CreateAsync(newRole);

                if (result.Succeeded)
                {
                    await _context.SaveChangesAsync();
                    return RoleCreation.Success;
                }
                else
                {
                    return RoleCreation.CreationError;
                }
            }

            return RoleCreation.RoleExist;
        }

        private async Task<IEnumerable<ApplicationUser>> GetUsersInrole(string roleName)
        {
            IEnumerable<ApplicationUser> usersInRole =
                await _userManager.GetUsersInRoleAsync(roleName);

            return usersInRole;
        }

        public async Task<IEnumerable<RoleObject>> GetRoles()
        {
            var roleList = new List<RoleObject>();
            var allRoles = _roleManager.Roles;

            foreach (var role in allRoles)
            {
                var roleObject = new RoleObject
                {
                    Name = role.Name,
                    RoleId = role.Id,
                    OriginDate = role.OriginDate,
                    Users = await GetUsersInrole(role.Name)
                };
                roleList.Add(roleObject);
            }
            return roleList.OrderBy(role => role.OriginDate);
        }

        public IEnumerable<SelectListItem> GetRolesAsSelectList(string currentRole)
        {
            var roleList = _context.Roles.AsNoTracking()
                .OrderBy(x => x.OriginDate)
                .Select(role => new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name,
                    Selected = role.Name == currentRole ? true: false
                }).ToList();

            var defaultChoice = new SelectListItem()
            {
                Value = null,
                Text = "--- Select Role ---"
            };

            roleList.Insert(0, defaultChoice);

            return roleList;
        }

        public async Task<ApplicationUser> GetUser(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UpdateUserEnum> UpdateUser(UpdateUserViewModel model)
        {
            var user = await GetUser(model.UserId);
            var role = _context.Roles.FirstOrDefault(x => x.Id == model.RoleName);
            model.RoleName = role.Name;


            if (user is null)
            {
                return UpdateUserEnum.NoUserToUpdate;
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.RoleName = model.RoleName;

            try
            {
                await Task.Run(() => _context.Users.Update(user)).ConfigureAwait(false);
                await _context.SaveChangesAsync();
                return UpdateUserEnum.Success;
            }
            catch (Exception)
            {
                return UpdateUserEnum.UpdateError;  
            }

        }
    }
}
