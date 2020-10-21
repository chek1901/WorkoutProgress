using AWO.ViewModels.Admin.Roles;
using AwoAppServices.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWO.Services.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<ApplicationUser>> GetUsersInrole(string roleName);
        Task <RoleCreation> CreateRole(string roleName);
        Task<IEnumerable<RoleObject>> GetRoles();
    }
}
