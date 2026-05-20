using ProjectManagement.Application.Dtos.Pagination;
using ProjectManagement.Application.Dtos.TaskDto;
using ProjectManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.IServices
{
    public interface ITaskService
    {
        Task<PagedList<TaskResponseDTO>> GetAllAsync(int pageNumber, int pageSize, string userId, string? searchTerm, TaskItemStatus? status, TaskPriority? priority);

        Task<TaskResponseDTO> GetByIdAsync(int id, string userId);

        Task<TaskResponseDTO> CreateAsync (CreateTaskDTO dto ,string userId);

        Task<TaskResponseDTO> UpdateAsync(int id, UpdateTaskDTO dto, string userId);

        Task<bool> DeleteAsync(int id ,string userId);
    }
}
