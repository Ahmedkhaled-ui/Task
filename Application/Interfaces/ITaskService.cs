using Application.DTOS.Tasks;

namespace Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskResponseDto> CreateTaskAsync(TaskCreateDto dto);
        Task<IEnumerable<TaskResponseDto>> GetTasksByProjectIdAsync(int projectId);
        Task<bool> UpdateTaskStatusAsync(int taskId, string status);
        Task<bool> DeleteTaskAsync(int taskId);
    }
}
