using MediatR;
using ProjectManagement.Application.Dtos.Pagination;
using ProjectManagement.Application.Dtos.ProjectDto;
using ProjectManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Features.Projects.Queries.GetAllProjects
{
    public  class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, PagedList<ProjectResponseDTO>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllProjectsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedList<ProjectResponseDTO>> Handle( GetAllProjectsQuery request,CancellationToken cancellationToken)
        {
            var query = _context.Projects
                .Where(p => p.UserId == request.UserId);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(p =>
                    p.Name.ToLower().Contains(request.SearchTerm.ToLower()));
            }

            var result = query
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new ProjectResponseDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt
                }).ToPagedListAsync(request.PageNumber,request.PageSize);

            return await result;
        }
    }

}
