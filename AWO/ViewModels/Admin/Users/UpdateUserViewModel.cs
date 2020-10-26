using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AWO.ViewModels.Admin
{
    public class UpdateUserViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string RoleName { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
