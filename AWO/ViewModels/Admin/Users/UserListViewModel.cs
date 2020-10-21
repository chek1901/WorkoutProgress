using AwoAppServices.Models;
using System.Collections.Generic;

namespace AWO.ViewModels.Admin
{
    public class UserListViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}
