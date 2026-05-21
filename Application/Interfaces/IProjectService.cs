using Application.DTOS.Projects;

namespace Application.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectResponseDto>> GetProjectsAsync();
        Task<ProjectResponseDto?> GetProjectByIdAsync(int id);
        Task<ProjectResponseDto> CreateProjectAsync(ProjectCreateDto dto);
        Task<bool> UpdateProjectAsync(int id, ProjectCreateDto dto);
        Task<bool> DeleteProjectAsync(int id);
    }
}
