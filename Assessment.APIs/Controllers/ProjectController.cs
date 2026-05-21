using Application.DTOS.Api; 
using Application.DTOS.Projects;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assessment.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectService.GetProjectsAsync();

            var response = ApiResponse<IEnumerable<ProjectResponseDto>>.SuccessResponse(projects, "Projects retrieved successfully.");
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);

            var response = ApiResponse<ProjectResponseDto>.SuccessResponse(project, $"Project with ID {id} retrieved successfully.");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _projectService.CreateProjectAsync(dto);

            var response = ApiResponse<ProjectResponseDto>.SuccessResponse(result, "Project created successfully.");
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProjectCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _projectService.UpdateProjectAsync(id, dto);

            var response = ApiResponse<string>.SuccessResponse("Updated", "Project updated successfully.");
            return Ok(response); 
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _projectService.DeleteProjectAsync(id);

            var response = ApiResponse<string>.SuccessResponse("Deleted", "Project deleted successfully.");
            return Ok(response);
        }
    }
}