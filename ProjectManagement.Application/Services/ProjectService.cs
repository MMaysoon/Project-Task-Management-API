using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.Dtos.Pagination;
using ProjectManagement.Application.Dtos.ProjectDto;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Application.IServices;
using ProjectManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Services
{
    public class ProjectService:IProjectService
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProjectService(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectResponseDTO> CreateAsync(CreateProjectDTO dto, string userId)
        {
            var exists= await _context.Projects.
                AnyAsync(p=> p.UserId == userId && p.Name.ToLower().Trim() == dto.Name.ToLower().Trim());

            if (exists)
                throw new Exception("Project name already exists");

            var project = _mapper.Map<Project>(dto);
            project.UserId = userId;

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectResponseDTO>(project);
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
           var project =await _context.Projects.FirstOrDefaultAsync(p=>p.Id == id&& p.UserId==userId);

            if (project == null)
                throw new Exception("Project not found");

             _context.Projects.Remove(project);

            await _context.SaveChangesAsync();

            return true;

            
        }

        public async Task<PagedList<ProjectResponseDTO>> GetAllAsync(int pageNumber, int pageSize, string userId, string? searchTerm)
        {
            var query = _context.Projects.Where(p => p.UserId == userId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query=query.Where(p=> p.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            var data = query
               .OrderByDescending(p => p.CreatedAt)
               .Select(p => new ProjectResponseDTO
               {
                   Id = p.Id,
                   Name = p.Name,
                   Description = p.Description,
                   CreatedAt = p.CreatedAt
               }).ToPagedListAsync(pageNumber, pageSize);

            return await data;
        }

        public async Task<ProjectResponseDTO> GetByIdAsync(int id, string userId)
        {
            var project = await _context.Projects
                 .FirstOrDefaultAsync(p =>
                     p.Id == id &&
                     p.UserId == userId);

            if (project == null)
                throw new Exception("Project not found");

            return _mapper.Map<ProjectResponseDTO>(project);
        }

        public async Task<ProjectResponseDTO> UpdateAsync(int id, UpdateProjectDTO dto, string userId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.UserId == userId);

            if (project == null)
                throw new Exception("Project not found");

            var exists = await _context.Projects.AnyAsync(p =>
                p.UserId == userId &&
                p.Id != id &&
                p.Name.ToLower().Trim() == dto.Name.ToLower().Trim());


            if (exists)
                throw new Exception("Project name already exists");

            _mapper.Map(dto, project);

            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectResponseDTO>(project);
        }
    }
}
