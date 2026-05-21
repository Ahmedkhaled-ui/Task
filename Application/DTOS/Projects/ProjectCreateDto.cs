using System.ComponentModel.DataAnnotations;

namespace Application.DTOS.Projects
{
    public class ProjectCreateDto
    {
        [Required(ErrorMessage = "Project name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
