namespace Domain.Entities
{
    public class Project : BaseEntities
    {
        public string Name { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty; 

        public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();

        public string UserId { get; set; } = string.Empty;
    }
}
