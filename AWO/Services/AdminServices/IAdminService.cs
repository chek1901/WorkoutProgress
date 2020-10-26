using AWO.ViewModels.Admin;
using AWO.ViewModels.Admin.Roles;
using AwoAppServices.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWO.Services.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<ApplicationUser>> GetUsersInrole(string roleName);
        Task<UpdateUserEnum> UpdateUser(UpdateUserViewModel model);

        Task <RoleCreation> CreateRole(string roleName);
        Task<IEnumerable<RoleObject>> GetRoles();
        IEnumerable<SelectListItem> GetRolesAsSelectList(string currentRole);
        Task<ApplicationUser> GetUser(string id);
    }
}
