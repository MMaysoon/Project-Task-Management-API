using MediatR;
using ProjectManagement.Application.Dtos.ProjectDto;


namespace ProjectManagement.Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommand : IRequest<ProjectResponseDTO>
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public string UserId { get; set; }
    }

}
