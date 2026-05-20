using MediatR;
using ProjectManagement.Application.Dtos.Pagination;
using ProjectManagement.Application.Dtos.ProjectDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Features.Projects.Queries.GetAllProjects
{
    public class GetAllProjectsQuery : IRequest<PagedList<ProjectResponseDTO>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SearchTerm { get; set; }

        public string UserId { get; set; }
    }

}
