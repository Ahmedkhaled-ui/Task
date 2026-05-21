using Application.DTOS.Projects;
using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Services
{
    public class ProjectService(IUnitOfWork _unitOfWork) : IProjectService
    {
        public async Task<IEnumerable<ProjectResponseDto>> GetProjectsAsync()
        {
            var projects = await _unitOfWork.GetRepository<Project>().GetAllAsync();

            if (projects == null) return Enumerable.Empty<ProjectResponseDto>();

            return projects.Select(p => new ProjectResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt
            });
        }

        public async Task<ProjectResponseDto> GetProjectByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Project ID must be greater than zero.", nameof(id));

            var project = await _unitOfWork.GetRepository<Project>().GetByIdAsync(id);

            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {id} was not found.");
            }

            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt
            };
        }

        public async Task<ProjectResponseDto> CreateProjectAsync(ProjectCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto), "Project data cannot be null");

            var allProjects = await _unitOfWork.GetRepository<Project>().GetAllAsync();
            var isNameExists = allProjects.Any(p => p.Name.Trim().ToLower() == dto.Name.Trim().ToLower());
            if (isNameExists)
            {
                throw new InvalidOperationException("A project with the same name already exists.");
            }

            var project = new Project
            {
                Name = dto.Name.Trim(),
                Description = dto.Description?.Trim() ?? string.Empty
            };

            _unitOfWork.GetRepository<Project>().Add(project);
            await _unitOfWork.SaveChangesAsync();

            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt
            };
        }

        public async Task<bool> UpdateProjectAsync(int id, ProjectCreateDto dto)
        {
            if (id <= 0) throw new ArgumentException("Project ID must be greater than zero.", nameof(id));
            if (dto == null) throw new ArgumentNullException(nameof(dto), "Project data cannot be null");

            var project = await _unitOfWork.GetRepository<Project>().GetByIdAsync(id);

            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {id} was not found.");
            }

            var allProjects = await _unitOfWork.GetRepository<Project>().GetAllAsync();
            var isNameExists = allProjects.Any(p => p.Id != id && p.Name.Trim().ToLower() == dto.Name.Trim().ToLower());
            if (isNameExists)
            {
                throw new InvalidOperationException("Another project with the same name already exists.");
            }

            project.Name = dto.Name.Trim();
            project.Description = dto.Description?.Trim() ?? string.Empty;

            _unitOfWork.GetRepository<Project>().Update(project);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Project ID must be greater than zero.", nameof(id));

            var project = await _unitOfWork.GetRepository<Project>().GetByIdAsync(id);

            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {id} was not found.");
            }

            _unitOfWork.GetRepository<Project>().Delete(project);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}