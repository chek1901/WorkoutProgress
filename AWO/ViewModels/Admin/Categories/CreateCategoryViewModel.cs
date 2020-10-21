using System.ComponentModel.DataAnnotations;

namespace AWO.ViewModels.Admin
{
    public class CreateCategoryViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Max 50 characters")]
        public string Name { get; set; }

    }
}
