using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.Dtos.Pagination;
using ProjectManagement.Application.Dtos.TaskDto;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Application.IServices;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Services
{
    public  class TaskItemService:ITaskService
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TaskItemService(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaskResponseDTO> CreateAsync(CreateTaskDTO dto, string userId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.UserId == userId && p.Id == dto.ProjectId);

            if (project == null)
                throw new Exception("project not found");

            var task = _mapper.Map<TaskItem>(dto);

            await _context.TaskItems.AddAsync(task);
            await _context.SaveChangesAsync();

            return _mapper.Map<TaskResponseDTO>(task);
            
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var task = await _context.TaskItems 
                .Include(t=>t.Project)
                .FirstOrDefaultAsync(t=>t.Id==id && t.Project.UserId==userId);

            if (task == null)
                throw new Exception("Task not found");


            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedList<TaskResponseDTO>> GetAllAsync(int pageNumber, int pageSize, string userId, string? searchTerm, TaskItemStatus? status, TaskPriority? priority)
        {
            var query = _context.TaskItems
                .Include(t => t.Project)
                .Where(t => t.Project.UserId == userId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower();

                query = query.Where(t =>
                t.Title.ToLower().Contains(term) ||
                t.Description.ToLower().Contains(term));

            }

            if (status.HasValue)
                query =query.Where(t=>t.Status==status);

            if (priority.HasValue)
                query=query.Where(t=>t.Priority==priority);

            var data = query
                .OrderByDescending(t => t.DueDate)
                .Select(t => new TaskResponseDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    DueDate = t.DueDate,
                    Status = t.Status,
                    Priority = t.Priority,
                    ProjectId = t.ProjectId
                }).ToPagedListAsync(pageNumber, pageSize);

            return await data;


        }

        public async Task<TaskResponseDTO> GetByIdAsync(int id, string userId)
        {
            var task = await _context.TaskItems
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id && t.Project.UserId == userId);

            if (task == null)
                throw new Exception("Task not found");

            return _mapper.Map<TaskResponseDTO>(task);
        }

        public async Task<TaskResponseDTO> UpdateAsync(int id, UpdateTaskDTO dto, string userId)
        {
            var task = await _context.TaskItems
             .Include(t => t.Project)
             .FirstOrDefaultAsync(t => t.Id == id && t.Project.UserId == userId);

            if (task == null)
                throw new Exception("Task not found");

            _mapper.Map(dto, task);

            await _context.SaveChangesAsync();

            return _mapper.Map<TaskResponseDTO>(task);
        }
    }
}
