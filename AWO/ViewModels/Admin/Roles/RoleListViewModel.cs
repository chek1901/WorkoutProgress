using AWO.ViewModels.Admin.Roles;
using System.Collections.Generic;

namespace AWO.ViewModels.Admin
{
    public class RoleListViewModel
    {
        public IEnumerable<RoleObject> Roles { get; set; }

        //public IEnumerable<ApplicationUser> UsersInRole { get; set; }

        public CreateRoleViewModel CreateRoleViewModel { get; set; }

    }
}
