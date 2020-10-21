using Microsoft.AspNetCore.Identity;

namespace AwoAppServices.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int GymUserId { get; set; }

        public string RoleName { get; set; }

    }
}
