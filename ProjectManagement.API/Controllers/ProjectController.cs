using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Common;
using ProjectManagement.Application.Dtos.Pagination;
using ProjectManagement.Application.Dtos.ProjectDto;
using ProjectManagement.Application.IServices;
using System.Security.Claims;

namespace ProjectManagement.API.Controllers
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

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not authenticated");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectDTO dto)
        {
            var result = await _projectService
                .CreateAsync(dto, GetUserId());

            return Ok(new ApiResponse<ProjectResponseDTO>
            {
                Success = true,
                Data = result,
                StatusCode = 200
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null)
        {
            var result = await _projectService
                .GetAllAsync(pageNumber, pageSize, GetUserId(), searchTerm);

            return Ok(new ApiResponse<PagedList<ProjectResponseDTO>>
            {
                Success = true,
                Data = result,
                StatusCode = 200
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _projectService
                .GetByIdAsync(id, GetUserId());

            return Ok(new ApiResponse<ProjectResponseDTO>
            {
                Success = true,
                Data = result,
                StatusCode = 200
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProjectDTO dto)
        {
            var result = await _projectService
                .UpdateAsync(id, dto, GetUserId());

            return Ok(new ApiResponse<ProjectResponseDTO>
            {
                Success = true,
                Data = result,
                StatusCode = 200
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _projectService.DeleteAsync(id, GetUserId());

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Data = "Deleted successfully",
                StatusCode = 200
            });
        }
    }
}
