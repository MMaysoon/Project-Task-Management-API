using AutoMapper;
using ProjectManagement.Application.Dtos.ProjectDto;
using ProjectManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Mappings
{
    public class ProjectProfile :Profile
    {
        public ProjectProfile() 
        {
            CreateMap<CreateProjectDTO, Project>();

            CreateMap<UpdateProjectDTO, Project>();

            CreateMap<Project, ProjectResponseDTO>();

        }
    }
}
