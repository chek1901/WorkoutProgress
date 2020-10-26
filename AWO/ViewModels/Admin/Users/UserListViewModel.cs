using AWO.ViewModels.Admin.Roles;
using AwoAppServices.Models;
using System.Collections.Generic;

namespace AWO.ViewModels.Admin
{
    public class UserListViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }

        public IEnumerable<ApplicationRole> Roles { get; set; }

        public IEnumerable<RoleObject> UsersInRole { get; set; }

    }

}
