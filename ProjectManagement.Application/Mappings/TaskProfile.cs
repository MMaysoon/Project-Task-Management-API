using AutoMapper;
using ProjectManagement.Application.Dtos.TaskDto;
using ProjectManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Mappings
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskItem, TaskResponseDTO>();
            CreateMap<CreateTaskDTO, TaskItem>();
            CreateMap<UpdateTaskDTO, TaskItem>();
        }
    }
}
