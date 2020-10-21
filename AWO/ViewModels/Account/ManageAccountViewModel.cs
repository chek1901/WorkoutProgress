using AwoAppServices.Models;
using System.ComponentModel.DataAnnotations;

namespace AWO.ViewModels.Account
{
    public class ManageAccountViewModel
    {
        public int GymUserId { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Telephone { get; set; }

        public ApplicationUser User { get; set; }
    }
}
