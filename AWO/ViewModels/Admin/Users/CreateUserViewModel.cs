using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AWO.ViewModels.Admin
{
    public class CreateUserViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string RoleName { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
