using Application.DTOS.Tasks;

namespace Application.DTOS.Projects
{
    public class ProjectWithTasksDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public ICollection<TaskResponseDto> Tasks { get; set; } = new List<TaskResponseDto>();
    }
}
