using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Common;
using ProjectManagement.Application.Dtos.Pagination;
using ProjectManagement.Application.Dtos.TaskDto;
using ProjectManagement.Application.IServices;
using ProjectManagement.Domain.Enums;
using System.Security.Claims;

namespace ProjectManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private string GetUserId()
            => User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        [HttpGet]
        public async Task<IActionResult> GetAll( int pageNumber = 1,int pageSize = 10,string? searchTerm = null,TaskItemStatus? status = null, TaskPriority? priority = null)
        {
            var result = await _taskService.GetAllAsync(pageNumber, pageSize, GetUserId(), searchTerm, status,priority);

            return Ok(new ApiResponse<PagedList<TaskResponseDTO>>
            {
                Success = true,
                Data = result,
                StatusCode = 200
            });
        }
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProjectId(int projectId, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _taskService.GetByProjectIdAsync(projectId, GetUserId(), pageNumber, pageSize);

            return Ok(new ApiResponse<PagedList<TaskResponseDTO>>
            {
                Success = true,
                Data = result,
                StatusCode = 200
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskDTO dto)
        {
            var result = await _taskService.CreateAsync(dto, GetUserId());

            return Ok(new ApiResponse<TaskResponseDTO>
            {
                Success = true,
                Data = result,
                StatusCode = 200
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTaskDTO dto)
        {
            var result = await _taskService.UpdateAsync(id, dto, GetUserId());

            return Ok(new ApiResponse<TaskResponseDTO>
            {
                Success = true,
                Data = result,
                StatusCode = 200
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskService.DeleteAsync(id, GetUserId());

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Data = "Deleted successfully",
                StatusCode = 200
            });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _taskService.GetByIdAsync(id, GetUserId());

            return Ok(new ApiResponse<TaskResponseDTO>
            {
                Success = true,
                Data = result,
                StatusCode = 200
            });
        }
    }

}
