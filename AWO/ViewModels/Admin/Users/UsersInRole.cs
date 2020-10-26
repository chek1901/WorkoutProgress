using AwoAppServices.Models;
using System.Collections.Generic;

namespace AWO.ViewModels.Admin.Users
{
    public class UserInRole
    {
        public string RoleName { get; set; }

        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}
