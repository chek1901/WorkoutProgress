using AwoAppServices.Models;
using System.Collections.Generic;

namespace AWO.ViewModels.Admin.Roles
{
    public class RoleObject
    {
        public string Name { get; set; }

        public string RoleId { get; set; }

        public IEnumerable<ApplicationUser> Users { get; set; }

    }
}
