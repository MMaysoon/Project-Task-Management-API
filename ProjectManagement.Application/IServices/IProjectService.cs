using ProjectManagement.Application.Dtos.Pagination;
using ProjectManagement.Application.Dtos.ProjectDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.IServices
{
    public interface IProjectService
    {
        Task<PagedList<ProjectResponseDTO>> GetAllAsync(int pageNumber, int pageSize, string userId, string? searchTerm);

        Task<ProjectResponseDTO> GetByIdAsync (int id , string userId);

        Task<ProjectResponseDTO> CreateAsync(CreateProjectDTO dto ,string userId);

        Task<ProjectResponseDTO> UpdateAsync(int id, UpdateProjectDTO dto,string userId );

        Task<bool> DeleteAsync(int id,string userId);
    }
}
