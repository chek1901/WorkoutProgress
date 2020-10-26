using Microsoft.AspNetCore.Identity;
using System;

namespace AwoAppServices.Models
{
    public class ApplicationRole : IdentityRole
    {
        public DateTime OriginDate { get; set; }
    }
}
