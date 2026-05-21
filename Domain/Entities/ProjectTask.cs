using Domain.Enums;

namespace Domain.Entities
{
    public class ProjectTask : BaseEntities
    {
        public string Title { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty; 
        public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.ToDo; 

        public DateTime? DueDate { get; set; } 
        public TaskPriority Priority { get; set; } = TaskPriority.Medium; 

        public int ProjectId { get; set; } 
        public Project Project { get; set; } = null!;
    }
}
