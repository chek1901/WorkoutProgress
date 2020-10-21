using System.ComponentModel.DataAnnotations;


namespace AWO.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [StringLength(15)]
        public string RoleName { get; set; }
    }
}
