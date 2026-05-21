using Application.DTOS.Api; 
using Application.DTOS.Tasks;
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
    public class TaskController(ITaskService _taskService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _taskService.CreateTaskAsync(dto);

            var response = ApiResponse<TaskResponseDto>.SuccessResponse(result, "Task created successfully.");
            return Created(string.Empty, response);
        }

        [HttpGet("project/{projectId:int}")]
        public async Task<IActionResult> GetByProject(int projectId)
        {
            var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);

            var response = ApiResponse<IEnumerable<TaskResponseDto>>.SuccessResponse(tasks, "Tasks retrieved successfully for this project.");
            return Ok(response);
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return BadRequest(ApiResponse<object>.FailureResponse("Status value cannot be empty."));

            await _taskService.UpdateTaskStatusAsync(id, status);

            var response = ApiResponse<string>.SuccessResponse("Updated", "Task status updated successfully.");
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskService.DeleteTaskAsync(id);

            var response = ApiResponse<string>.SuccessResponse("Deleted", "Task deleted successfully.");
            return Ok(response);
        }
    }
}