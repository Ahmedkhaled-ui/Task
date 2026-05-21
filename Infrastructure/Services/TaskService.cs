using Application.DTOS.Tasks;
using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Services
{
    public class TaskService(IUnitOfWork _unitOfWork) : ITaskService
    {
        public async Task<TaskResponseDto> CreateTaskAsync(TaskCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var project = await _unitOfWork.GetRepository<Project>().GetByIdAsync(dto.ProjectId);
            if (project == null)
            {
                throw new InvalidOperationException($"Project with ID {dto.ProjectId} does not exist.");
            }

            var task = new ProjectTask
            {
                Title = dto.Title.Trim(),
                Description = dto.Description?.Trim() ?? string.Empty,
                Status = dto.Status,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                ProjectId = dto.ProjectId
            };

            _unitOfWork.GetRepository<ProjectTask>().Add(task);
            await _unitOfWork.SaveChangesAsync();

            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                DueDate = task.DueDate ?? DateTime.UtcNow,
                CreatedAt = task.CreatedAt,
                ProjectId = task.ProjectId
            };
        }

        public async Task<IEnumerable<TaskResponseDto>> GetTasksByProjectIdAsync(int projectId)
        {
            if (projectId <= 0) return Enumerable.Empty<TaskResponseDto>();

            var project = await _unitOfWork.GetRepository<Project>().GetByIdAsync(projectId);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {projectId} was not found.");
            }

            var allTasks = await _unitOfWork.GetRepository<ProjectTask>().GetAllAsync();
            var projectTasks = allTasks.Where(t => t.ProjectId == projectId);

            return projectTasks.Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                DueDate = t.DueDate ?? DateTime.UtcNow,
                CreatedAt = t.CreatedAt,
                ProjectId = t.ProjectId
            });
        }

        public async Task<bool> UpdateTaskStatusAsync(int taskId, string status)
        {
            if (taskId <= 0) throw new ArgumentException("Task ID must be greater than zero.", nameof(taskId));
            if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Status cannot be empty.", nameof(status));

            var task = await _unitOfWork.GetRepository<ProjectTask>().GetByIdAsync(taskId);

            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {taskId} was not found.");
            }

            if (Enum.TryParse<Domain.Enums.TaskStatus>(status, true, out var parsedStatus))
            {
                task.Status = parsedStatus;
            }
            else
            {
                throw new InvalidOperationException($"Invalid status value. Allowed values are: Pending, InProgress, Completed.");
            }

            _unitOfWork.GetRepository<ProjectTask>().Update(task);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            if (taskId <= 0) throw new ArgumentException("Task ID must be greater than zero.", nameof(taskId));

            var task = await _unitOfWork.GetRepository<ProjectTask>().GetByIdAsync(taskId);

            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {taskId} was not found.");
            }

            _unitOfWork.GetRepository<ProjectTask>().Delete(task);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}