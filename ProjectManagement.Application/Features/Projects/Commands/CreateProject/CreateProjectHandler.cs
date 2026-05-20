

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.Dtos.ProjectDto;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectHandler:IRequestHandler<CreateProjectCommand,ProjectResponseDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateProjectHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async  Task<ProjectResponseDTO> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var exists = await _context.Projects.AnyAsync(p =>
                p.UserId == request.UserId &&
                p.Name.ToLower().Trim() == request.Name.ToLower().Trim());

            if (exists)
                throw new Exception("Project name already exists");

            var project = new Project
            {
                Name = request.Name,
                Description = request.Description,
                UserId = request.UserId
            };

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ProjectResponseDTO>(project);
        }
    }
}
